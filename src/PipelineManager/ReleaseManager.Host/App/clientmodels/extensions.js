define(['http'], function (ko) {

    var stage = function(stageDto, isLast) {
        this.name = ko.observable(stageDto.name);
        this.latestVersion = ko.observable(stageDto.latestVersion);
        this.latestBuildLink = stageDto.latestBuild;
        this.activities = ko.observableArray([]);
        this.isLast = ko.observable(isLast);

        for (var i = 0; i < stageDto.activities.length; i++) {
            this.activities.push(new activity(stageDto.activities[i]));
        }
    }

    var activity = function(activityDto) {
        this.name = ko.observable(activityDto.name);
        this.busy = ko.observable(activityDto.busy);
    }

    return function pipeline(pipelineDto) {
        this.name = ko.observable(pipelineDto.name);
        this.stages = ko.observableArray();

        if (pipelineDto.stages) {
            for (var i = 0; i < pipelineDto.stages.length; i++) {
                this.stages.push(new stage(pipelineDto.stages[i], i === pipelineDto.stages.length - 1));
            }
        }
    };
});