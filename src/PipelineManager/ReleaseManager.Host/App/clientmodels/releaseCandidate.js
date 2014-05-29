define(['knockout'], function (ko) {

    return function releaseCandidate(releaseCandidateDto) {
        this.id = ko.observable(releaseCandidateDto.id);
        this.version = ko.observable(releaseCandidateDto.version);
        this.projectName = ko.observable(releaseCandidateDto.projectName);
        this.deployedTo = ko.observableArray(releaseCandidateDto.deployedTo);
    };
});