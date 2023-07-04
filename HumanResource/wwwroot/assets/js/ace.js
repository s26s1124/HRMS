$(function() {
  'use strict';

  if ($('#ace_html').length) {
    $(function() {
      var editor = ace.edit("ace_html");
      editor.setTheme("ace/theme/dracula");
      editor.getSession().setMode("ace/mode/html");
      editor.setOption("showPrintMargin", false)
      var textarea = $('textarea[name="contact_us"]');
      editor.getSession().on("change", function () {
          textarea.val(editor.getSession().getValue());
      });
    });
  }

  if ($('#ace_html2').length) {
    $(function() {
      var editor = ace.edit("ace_html2");
      editor.setTheme("ace/theme/dracula");
      editor.getSession().setMode("ace/mode/html");
      editor.setOption("showPrintMargin", false)
      var textarea = $('textarea[name="about_us"]');
      editor.getSession().on("change", function () {
          textarea.val(editor.getSession().getValue());
      });
    });
  }
  if ($('#ace_scss').length) {
    $(function() {
      var editor = ace.edit("ace_scss");
      editor.setTheme("ace/theme/dracula");
      editor.getSession().setMode("ace/mode/scss");
      editor.setOption("showPrintMargin", false)
      var textarea = $('textarea[name="css"]');
      editor.getSession().on("change", function () {
          textarea.val(editor.getSession().getValue());
      });
    });
  }
  if ($('#ace_javaScript').length) {
    $(function() {
      var editor = ace.edit("ace_javaScript");
      editor.setTheme("ace/theme/dracula");
      editor.getSession().setMode("ace/mode/javascript");
      editor.setOption("showPrintMargin", false)
      var textarea = $('textarea[name="js"]');
      editor.getSession().on("change", function () {
          textarea.val(editor.getSession().getValue());
      });
    });
  }

});