define(['knockout', 'jquery', 'plugins/http', 'json!api/extensions'], function (ko, $, http, extensions) {

    var deployment = function(deploymentDto) {
        
        var style;
        if (deploymentDto.success) {
            style = 'list-group-item-success';
        } else {
            style = 'list-group-item-danger';
        }

        this.uniqueId = ko.observable(deploymentDto.uniqueId);
        this.style = ko.observable(style);
        this.environment = ko.observable(deploymentDto.environment);
        this.detailsUrl = ko.observable(deploymentDto.detailsUrl);
        this.date = ko.observable(deploymentDto.deploymentDate);
    }

    return {
        deployments: ko.observableArray(),
        extensions : ko.observableArray(extensions.deploymentDetailsExtensions),
        activate: function (link) {
            if (!link) {
                return;
            }
            var self = this;
            http.get(link.url).then(function (data) {
                self.deployments.removeAll();
                for (var i = 0; i < data.length; i++) {
                    self.deployments.push(new deployment(data[i]));
                }
            });
        }
    };
});