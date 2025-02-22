var $url = "/dashboard";

var $practiceUrl = "/exam/examPractice/submit";

var data = utils.init({
  user: null,
  passSeries: [0],
  passChartOptions: {
    chart: {
      title: 'testament',
      type: 'radialBar',
      toolbar: {
        show: false
      }
    },
    plotOptions: {
      radialBar: {
        startAngle: -135,
        endAngle: 225,
        hollow: {
          margin: 0,
          size: '70%',
          background: '#fff',
          image: undefined,
          imageOffsetX: 0,
          imageOffsetY: 0,
          position: 'front',
          dropShadow: {
            enabled: true,
            top: 3,
            left: 0,
            blur: 4,
            opacity: 0.24
          }
        },
        track: {
          background: '#fff',
          strokeWidth: '88%',
          margin: 0, // margin is in pixels
          dropShadow: {
            enabled: true,
            top: -3,
            left: 0,
            blur: 4,
            opacity: 0.35
          }
        },

        dataLabels: {
          show: true,
          name: {
            offsetY: -20,
            show: true,
            color: '#ff6a00',
            fontSize: '18px'
          },
          value: {
            formatter: function (val) {
              return parseInt(val) + '%';
            },
            color: '#000',
            fontSize: '28px',
            show: true,
          }
        }
      }
    },
    fill: {
      colors: ['#19cb98']
    },
    stroke: {
      lineCap: 'round'
    },
    labels: ['考试及格率'],
  },

  allPercent: 0,
  examTotal: 0,
  examPercent: 0,
  examMoniTotal: 0,
  examMoniPercent: 0,

  practiceAnswerTmTotal: 0,
  practiceAnswerPercent: 0,
  practiceAllTmTotal: 0,
  practiceAllPercent: 0,
  practiceCollectTmTotal: 0,
  practiceCollectPercent: 0,
  practiceWrongTmTotal: 0,
  practiceWrongPercent: 0,

  examPaper: null,
  examMoni: null,

  knowList:null,

  topCer: null,
  dateStr: null,

  cerList: null,
  todayExam: null,
  taskPaperList: null,
  taskQList: null,
  taskAssList: null,
  taskTotal: 0,
  version: 'v8.0'
});

var methods = {
  apiGet: function () {
    var $this = this;


    utils.loading(this, true);
    $api.get($url, { params: { isApp: false } }).then(function (response) {
      var res = response.data;

      $this.user = res.user;

      $this.examTotal = res.examTotal;
      $this.examPercent = res.examPercent;
      $this.examMoniTotal = res.examMoniTotal;
      $this.examMoniPercent = res.examMoniPercent;

      $this.practiceAllPercent = res.practiceAllPercent;
      $this.practiceAllTmTotal = res.practiceAllTmTotal;

      $this.practiceAnswerTmTotal = res.practiceAnswerTmTotal;
      $this.practiceAnswerPercent = res.practiceAnswerPercent;

      $this.practiceWrongTmTotal = res.practiceWrongTmTotal;
      $this.practiceWrongPercent = res.practiceWrongPercent;

      $this.practiceCollectTmTotal = res.practiceCollectTmTotal;
      $this.practiceCollectPercent = res.practiceCollectPercent;

      $this.examPaper = res.examPaper;
      $this.examMoni = res.examMoni;

      $this.topCer = res.topCer;
      $this.dateStr = res.dateStr;

      $this.cerList = res.cerList;
      $this.knowList = res.knowList;
      $this.todayExam = res.todayExam;
      $this.taskPaperList = res.taskPaperList;
      $this.taskQList = res.taskQList;
      $this.taskAssList = res.taskAssList;
      $this.taskTotal = res.taskTotal;

      $this.version = res.version;

      setTimeout(function () {
        $this.passSeries = [100];
      }, 1000);

      setTimeout(function () {
        $this.passSeries = [res.allPercent];
      }, 2000);

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnUserMenuClick: function (command) {
    top.$vue.btnUserMenuClick(command);
  },
  btnMoreMenuClick: function (command) {
    top.$vue.btnTopMenuClick(command);
  },
  btnCreatePracticeClick: function (practiceType) {
    if (practiceType === 'All') {
      if (this.practiceAllTmTotal > 0) {
        this.apiCreatePractice(practiceType);
      }
      else {
        utils.error("没有题目可以练习");
      }
    }
    if (practiceType === 'Collect') {
      if (this.practiceCollectTmTotal > 0) {
        this.apiCreatePractice(practiceType);
      }
      else {
        utils.error("没有题目可以练习");
      }
    }
    if (practiceType === 'Wrong') {
      if (this.practiceWrongTmTotal > 0) {
        this.apiCreatePractice(practiceType);
      }
      else {
        utils.error("没有题目可以练习");
      }
    }
  },
  apiCreatePractice: function (practiceType, groupId) {
    var $this = this;

    utils.loading(this, true, "正在创建练习...");

    $api.post($practiceUrl, { practiceType: practiceType }).then(function (response) {
      var res = response.data;

      if (res.success) {
        $this.goPractice(res.id);
      }
      else {
        utils.error(res.error);
      }

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  goPractice: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examPracticing', { id: id }),
      width: "68%",
      height: "88%",
      end: function () {
        $this.apiGet();
      }
    });
  },
  btnViewKnowClick: function (row) {
    utils.openTopLeft(row.name, utils.getKnowledgesUrl("knowledgesView", { id: row.id }));
  },
  btnViewAssClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examAssessmenting', { id: id }),
      width: "100%",
      height: "100%",
      end: function () {
        $this.apiGet();
      }
    });
  },
  btnViewQClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examQuestionnairing', { id: id }),
      width: "100%",
      height: "100%",
      end: function () {
        $this.apiGet();
      }
    });
  },
  btnViewPaperClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examPaperInfo', { id: id }),
      width: "78%",
      height: "98%",
      end: function () {
        $this.apiGet();
      }
    });
  },
  btnViewCer: function (row) {
    top.utils.openLayerPhoto({
      title: row.name,
      id: row.id,
      src: row.cerImg + '?r=' + Math.random()
    })
  },
  btnEventClick: function () {
    utils.openTopLeft("考试日程", utils.getRootUrl("event"));
  }
};
Vue.component("apexchart", {
  extends: VueApexCharts
});
var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  },
});
