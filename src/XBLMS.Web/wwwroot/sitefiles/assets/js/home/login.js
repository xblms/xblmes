var $url = '/login';
var $urlCaptcha = '/login/captcha';
var $urlCaptchaCheck = '/login/captcha/actions/check';
var $urlSendSms = '/login/actions/sendSms';

var data = utils.init({
  status: utils.getQueryInt('status'),
  pageAlert: null,
  captchaToken: null,
  captchaUrl: null,
  isUserCaptchaDisabled: false,
  countdown: 0,
  form: {
    type: 'account',
    account: null,
    password: null,
    mobile: null,
    isPersistent: false,
    captchaValue: null,
  },
  homeTitle: DOCUMENTTITLE_HOME,
  returnUrl: utils.getQueryString('returnUrl')
});

var methods = {
  apiGet: function () {
    var $this = this;

    if (this.status === 401) {
      this.pageAlert = {
        type: 'error',
        title: '您的账号登录已过期或失效，请重新登录'
      };
    }

    utils.loading(this, true);
    $api.get($url).then(function (response) {
      var res = response.data;

      $this.isUserCaptchaDisabled = res.isUserCaptchaDisabled;
      if ($this.isUserCaptchaDisabled) {
        $this.btnTypeClick();
      } else {
        $this.apiCaptcha();
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiCaptcha: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($urlCaptcha).then(function (response) {
      var res = response.data;

      $this.captchaToken = res.value;
      $this.captchaUrl = $apiUrl + $urlCaptcha + '?token=' + $this.captchaToken;
      $this.btnTypeClick();
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiCaptchaCheck: function () {
    var $this = this;
    if (this.isUserCaptchaDisabled) {
      $this.apiSubmit();
    } else {
      utils.loading(this, true);
      $api.post($urlCaptchaCheck, {
        token: this.captchaToken,
        value: this.form.captchaValue
      }).then(function (response) {
        $this.apiSubmit();
      }).catch(function (error) {
        $this.apiCaptcha();
        utils.loading($this, false);
        utils.error(error);
      });
    }
  },

  apiSubmit: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($url, {
      account: this.form.account,
      password: this.form.password ? md5(this.form.password) : '',
      mobile: this.form.mobile,
      isPersistent: this.form.isPersistent
    }).then(function (response) {
      var res = response.data;

      localStorage.removeItem(ACCESS_TOKEN_NAME);
      localStorage.setItem(ACCESS_TOKEN_NAME, res.token);
      if (res.redirectToVerifyMobile) {
        location.href = utils.getRootUrl('verifyMobile', {
          returnUrl: $this.returnUrl
        });
      } else if ($this.returnUrl) {
        location.href = $this.returnUrl;
      } else {
        $this.redirectIndex();
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      $this.apiCaptcha();
      utils.loading($this, false);
    });
  },

  redirectIndex: function () {
    location.href = utils.getIndexUrl();
  },

  btnTypeClick: function() {
    var $this = this;

    this.$refs.formAccount && this.$refs.formAccount.clearValidate();
    this.$refs.formMobile && this.$refs.formMobile.clearValidate();
    if (this.form.type == 'account') {
      setTimeout(function () {
        $this.$refs['account'].focus();
      }, 100);
    } else if (this.form.type == 'mobile') {
      setTimeout(function () {
        $this.$refs['mobile'].focus();
      }, 100);
    }
  },

  btnCaptchaClick: function () {
    this.apiCaptcha();
  },

  isMobile: function (value) {
    return /^1[3-9]\d{9}$/.test(value);
  },
  btnSubmitClick: function () {
    var $this = this;

    if (this.form.type == 'account') {
      this.$refs.formAccount.validate(function(valid) {
        if (valid) {
          $this.apiCaptchaCheck();
        }
      });
    } else {
      this.$refs.formMobile.validate(function(valid) {
        if (valid) {
          $this.apiSubmit();
        }
      });
    }
  },

  btnRegisterClick: function(e) {
    e.preventDefault();
    location.href = utils.getRootUrl('register');
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
    created: function() {
    document.title=DOCUMENTTITLE_HOME+'-登录';
    this.apiGet();
  }
});
