define(['durandal/system', './transitionHelper'], function (system, helper) {
    var settings = {
            inAnimation: 'fadeInLeftBig',
            outAnimation: 'fadeOutRightBig'
        },
        fadeIn = function(context) {
            system.extend(context, settings);
            return helper.create(context);
        };

    return fadeIn;
});