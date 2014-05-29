define(['knockout'], function (ko) {

    var activity = function (activityDto) {
        this.name = ko.observable(activityDto.name);
        this.busy = ko.observable(activityDto.busy);
    }

    return function (stageDto, isLast) {
        var successful = stageDto.latestVersion === stageDto.latestSuccessfulVersion;


        this.name = ko.observable(stageDto.name);
        this.latestVersion = ko.observable(stageDto.latestVersion);
        this.latestSuccessfulVersion = ko.observable(stageDto.latestSuccessfulVersion);
        this.latestBuildLink = stageDto.latestBuild;
        this.latestSuccessfulBuildLink = stageDto.latestSuccessfulBuild;
        this.activities = ko.observableArray([]);
        this.isLast = ko.observable(isLast);
        this.successful = ko.observable(successful);

        var style;
        var latestIcon;
        var latestLinkStyle;
        if (successful) {
            style = 'element-success';
            latestLinkStyle = 'version-successful';
            latestIcon = 'glyphicon-ok-circle';
        } else {
            style = 'element-failed';
            latestLinkStyle = 'version-failed';
            latestIcon = 'glyphicon-exclamation-sign';
        }
        this.latestIcon = ko.observable(latestIcon);
        this.style = ko.observable(style);
        this.latestLinkStyle = ko.observable(latestLinkStyle);

        for (var i = 0; i < stageDto.activities.length; i++) {
            this.activities.push(new activity(stageDto.activities[i]));
        }
    };
});