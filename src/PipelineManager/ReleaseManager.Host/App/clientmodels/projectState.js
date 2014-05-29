define(['knockout'], function (ko) {

    return function projectState(projectStateDto) {
        this.name = ko.observable(projectStateDto.name);

        var healthClass;
        if (projectStateDto.healthStatus == 'OK') {
            healthClass = 'list-group-item-success';
        } else if (projectStateDto.healthStatus == 'Warning') {
            healthClass = 'list-group-item-warning';
        } else {
            healthClass = 'list-group-item-danger';
        }

        this.healthClass = ko.observable(healthClass);
        this.latestVersion = ko.observable(projectStateDto.latestVersion);
        this.pipelineLink = projectStateDto.pipeline;
    };
});