﻿var $url = '/settings/logsError';
var $urlExport = $url + '/actions/export';
var $urlDelete = $url + '/actions/delete';

var data = utils.init({
  items: null,
  count: null,
  categories: null,
  formInline: {
    dateFrom: '',
    dateTo: '',
    category: '',
    keyword: '',
    currentPage: 1,
    offset: 0,
    limit: 30
  }
});

var methods = {
  getConfig: function () {
    var $this = this;

    $api.post($url, this.formInline).then(function (response) {
      var res = response.data;

      $this.items = res.items;
      $this.count = res.count;
      $this.categories = res.categories;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiDelete: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($urlDelete).then(function (response) {
      var res = response.data;

      $this.items = [];
      $this.count = 0;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnDeleteClick: function () {
    var $this = this;

    top.utils.alertDelete({
      title: '清空系统错误日志',
      text: '此操作将会清空系统错误日志，确定吗？',
      button: '清 空',
      callback: function () {
        $this.apiDelete();
      }
    });
  },

  btnLogView: function(logId) {
    top.utils.openLayer({
      title: false,
      closebtn:0,
      url: utils.getRootUrl('errorView', { logId: logId }),
      width: '80%',
      height: '80%'
    });
  },

  btnExportClick() {
    var $this = this;

    var body = _.assign({}, this.formInline);
    body.currentPage = 1;
    body.offset = 0;
    body.limit = 0;

    utils.loading(this, true);
    $api.post($urlExport, body).then(function (response) {
      var res = response.data;

      window.open(res.value);
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnSearchClick() {
    var $this = this;

    this.formInline.currentPage = 1;
    this.formInline.offset = 0;
    this.formInline.limit = 30;

    utils.loading(this, true);
    $api.post($url, this.formInline).then(function (response) {
      var res = response.data;

      $this.items = res.items;
      $this.count = res.count;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  handleCurrentChange: function(val) {
    var $this = this;

    this.formInline.currentValue = val;
    this.formInline.offset = this.formInline.limit * (val - 1);

    utils.loading(this, true);
    $api.post($url, this.formInline).then(function (response) {
      var res = response.data;

      $this.items = res.items;
      $this.count = res.count;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
    window.scrollTo(0, 0);
  },

  btnCloseClick: function() {
    utils.removeTab();
  },
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.getConfig();
  }
});
