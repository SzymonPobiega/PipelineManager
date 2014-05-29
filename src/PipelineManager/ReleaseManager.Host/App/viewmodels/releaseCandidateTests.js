define(['knockout', 'jquery','plugins/http','clientmodels/testsuite'], function (ko, $, http, testSuite) {


    return {
        suites: ko.observableArray(),

        activate: function (link) {
            if (!link) {
                return;
            }
            var self = this;
            http.get(link.url).then(function (data) {
                self.suites.removeAll();
                for (var i = 0; i < data.suites.length; i++) {
                    self.suites.push(new testSuite(data.suites[i]));
                }
            });
        }
    };
});