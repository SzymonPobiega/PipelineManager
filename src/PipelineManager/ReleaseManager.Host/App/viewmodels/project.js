define(['plugins/http', 'clientmodels/stage', './restfulRouter'], function (http, stage, restfulRouter) {

    var refresh = function(self, locator) {
        http.get(locator).then(function (data) {

            if (data.stages) {
                self.stages.removeAll();
                for (var i = 0; i < data.stages.length; i++) {
                    self.stages.push(new stage(data.stages[i], i === data.stages.length - 1));
                }
            }

            self.displayName(data.name);
            self.relativeStageWidth((100 / self.stages().length) + '%');
        });
    }

    return {
        displayName: ko.observable(),
        stages : ko.observableArray(),
        relativeStageWidth: ko.observable(),

        activate: function (link) {
            var url = (typeof link) === 'string'
                ? decodeURIComponent(link)
                : link.url;
            var self = this;
            refresh(self, url);
            window.setInterval(function () {
                //refresh(self, url);
            }, 500);
        },
        displayReleaseFor: function(selectedStage, link) {
            restfulRouter.open(link);
        }
    };
});