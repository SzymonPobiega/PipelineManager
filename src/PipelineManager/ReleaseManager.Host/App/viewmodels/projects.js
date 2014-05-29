define(['clientmodels/projectState','plugins/http','./restfulRouter'], function (projectState, http, restfulRouter) {

    var refresh = function (projectList) {
        http.get('projects').then(function (model) {
            projectList.removeAll();
            for (var i = 0; i < model.projects.length; i++) {
                projectList.push(new projectState(model.projects[i]));
            }
        });
    }

    return {
        displayName: 'Projects',
        projects: ko.observableArray(),
        activate: function () {
            var self = this;
            refresh(self.projects);            
        },
        showPipeline: function(project) {
            restfulRouter.open(project.pipelineLink);
        }
    };
});