define(['knockout', 'jquery', 'plugins/http'], function (ko, $, http) {

    return {
        visible : ko.observable(false),
        detailsUrl: ko.observable(),
        activate: function (deploymentId) {
            var self = this;
            if (!deploymentId) {
                return;
            }
            http.get('octopusdeploymentdetails/'+deploymentId).then(function (data) {
                self.detailsUrl(data.url);
                self.visible(true);
            });
        }
    };
});