var $url = 'exam/examTm/tmEdit/get';
var $urlsubmit = 'exam/examTm/tmEdit/submit';

var data = utils.init({
  id: utils.getQueryInt("id"),
  treeId: utils.getQueryInt("treeId"),
  txList: [],
  tmTreeData: [],
  form: null,
  btnMinusOptionsShow: false,
  curBaseTx:null,
  options: ['正确', '错误'],
  optionsValue: '',
  optionsEditShow: [],
  optionsEditReloaded: []
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($url, { params: { id: this.id } }).then(function (response) {
      var res = response.data;
      var tm = res.item;
      $this.tmTreeData = res.tmTree;
      $this.txList = res.txList;

      $this.form = _.assign({}, tm);
      if (tm.id > 0) {
        $this.options = tm.options;
        $this.optionsValue = tm.answer;

        $this.curBaseTx= $this.txFormatter($this.form.txId);
        if ($this.curBaseTx === 'Duoxuanti') {
          $this.optionsValue = $this.form.optionsValues;
        }
      }
      else {
        $this.form.txId = null;
        $this.form.treeId = null;
        $this.form.nandu = 1;

        if ($this.treeId > 0) {
          $this.form.treeId = $this.treeId;
        }
      }

    }).catch(function (error) {
      utils.error(error, { layer:true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnAddOptions: function () {
    this.options.push('');
    this.btnMinusOptionsShow = true;
    this.optionsEditShow.push(false);
  },
  btnMinusOptions: function () {
    if (this.options != null) {
      var optionsCount = this.options.length;
      if (optionsCount > 4) {
        this.options.splice(optionsCount - 1, 1);
        this.optionsEditShow.splice(optionsCount - 1, 1);
      }
      var optionsCount = this.options.length;
      if (optionsCount <= 4) {
        this.btnMinusOptionsShow = false;
      }
    }
  },
  btnSubmitClick: function () {
    var $this = this;

    this.$refs.form.validate(function (valid) {
      if (valid) {

        if ($this.curBaseTx === 'Tiankongti') {
          if (!utils.contains($this.form.title, "___")) {
            utils.error('填空题必须包含占位符三个英文全角下划线”___“', { layer:true });
            return;
          }
        }
        if ($this.curBaseTx === 'Danxuanti' || $this.curBaseTx === 'Duoxuanti' || $this.curBaseTx === 'Panduanti') {
          if ($this.optionsValue === '' || $this.optionsValue === null || $this.optionsValue === []) {
            utils.error('请选择正确答案', { layer: true });
            return;
          }
          if ($this.options != null) {
            if ($this.options.length > 0) {
              var isTrue = true;
              for (var i = 0; i < $this.options.length; i++) {
                if ($this.options[i].trim() === '') {
                  isTrue = false;
                  break;
                }
              }
              if (!isTrue) {
                utils.error('请完善候选项内容', { layer: true });
                return;
              }
            }
            else {
              utils.error('请完善候选项内容', { layer: true });
              return;
            }
          }
          $this.form.answer = $this.optionsValue;
          $this.form.options = $this.options;
          $this.form.optionsValues = $this.optionsValue;
        }
    

        $this.apiSubmit();
      }
    });
  },
  apiSubmit: function () {
    var $this = this;
    utils.loading(this, true);

    $api.post($urlsubmit, { item: this.form }
    ).then(function (response) {
      var res = response.data;
      if (res.value) {
        if ($this.id > 0) {
          utils.success("成功修改题目")
        }
        else {
          utils.success("成功新增题目")
        }
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
      utils.closeLayer(false);
    });
  },
  txFormatter: function (id) {
    let tx = this.txList.find(item => item.id === id);
    return tx.examTxBase;
  },
  txChange: function (value) {
    this.form.answer = "";
    this.form.jiexi = "";

    this.curBaseTx = this.txFormatter(value);
    this.options = ['', '', '', ''];
    this.optionsValue = 'A';
    if (this.curBaseTx === 'Danxuanti') { this.optionsValue = 'A'; this.optionsEditShow = [false, false, false, false]; }
    else if (this.curBaseTx === 'Duoxuanti') { this.optionsValue = ['A', 'B', 'C', 'D']; this.optionsEditShow = [false, false, false, false]; }
    else if (this.curBaseTx === 'Panduanti') { this.options = ['正确', '错误']; this.optionsValue = 'A'; }
    else { this.optionsValue = "";}
    this.optionsEditReloaded = [];
  },
  btnOpenEditClick: function (ref,ptype) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('editorTmOpenLayer', { pf: window.name, ptype: ptype, ref: ref }),
      width: "58%",
      height: "78%"
    });
  },
  setOptionsValue: function (index,value) {
    this.$set(this.options, index, value);
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});
