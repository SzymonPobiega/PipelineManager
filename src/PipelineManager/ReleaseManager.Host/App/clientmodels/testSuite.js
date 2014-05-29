define(['knockout'], function (ko) {

    var testCase = function(testCaseDto) {
        this.name = ko.observable(testCaseDto.name);

        var resultStyle;
        if (testCaseDto.result == 'Success') {
            resultStyle = 'list-group-item-success';
        } else if (testCaseDto.result == 'Inconclusive') {
            resultStyle = 'list-group-item-warning';
        } else {
            resultStyle = 'list-group-item-danger';
        }

        this.resultStyle = ko.observable(resultStyle);
    }

    return function(testSuiteDto) {
        this.type = ko.observable(testSuiteDto.type);

        var resultStyle;
        if (testSuiteDto.result == 'Success') {
            resultStyle = 'panel-success';
        } else if (testSuiteDto.result == 'Inconclusive') {
            resultStyle = 'panel-warning';
        } else { //Inconclusive
            resultStyle = 'panel-danger';
        }

        this.resultStyle = ko.observable(resultStyle);
        this.testCases = ko.observableArray();
        this.testCount = ko.observable(testSuiteDto.testCases.length);

        for (var i = 0; i < testSuiteDto.testCases.length; i++) {
            this.testCases.push(new testCase(testSuiteDto.testCases[i]));
        }
    }
});