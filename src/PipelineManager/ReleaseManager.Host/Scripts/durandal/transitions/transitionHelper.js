define(['durandal/system', 'jquery'], function (system, $) {

    var animationTypes = [
      'bounce',
      'bounceIn',
      'bounceInDown',
      'bounceInLeft',
      'bounceInRight',
      'bounceInUp',
      'bounceOut',
      'bounceOutDown',
      'bounceOutLeft',
      'bounceOutRight',
      'bounceOutUp',
      'fadeIn',
      'fadeInDown',
      'fadeInDownBig',
      'fadeInLeft',
      'fadeInLeftBig',
      'fadeInRight',
      'fadeInRightBig',
      'fadeInUp',
      'fadeInUpBig',
      'fadeOut',
      'fadeOutDown',
      'fadeOutDownBig',
      'fadeOutLeft',
      'fadeOutLeftBig',
      'fadeOutRight',
      'fadeOutRightBig',
      'fadeOutUp',
      'fadeOutUpBig',
      'flash',
      'flip',
      'flipInX',
      'flipInY',
      'flipOutX',
      'flipOutY',
      'hinge',
      'lightSpeedIn',
      'lightSpeedOut',
      'pulse',
      'rollIn',
      'rollOut',
      'rotateIn',
      'rotateInDownLeft',
      'rotateInDownRight',
      'rotateInUpLeft',
      'rotateInUpRight',
      'rotateOut',
      'rotateOutDownLeft',
      'rotateOutDownRight',
      'rotateOutUpLeft',
      'roateOutUpRight',
      'shake',
      'swing',
      'tada',
      'wiggle',
      'wobble'
    ];

    return App = {
        duration: 1000 * .35, // seconds
        create: function (settings) {
            settings = ensureSettings(settings);
            return doTrans(settings);
        }
    };

    function animValue(type) {
        return Object.prototype.toString.call(type) == '[object String]' ? type : animationTypes[type];
    }

    function ensureSettings(settings) {
        settings.inAnimation = settings.inAnimation || 'fadeInRight';
        settings.outAnimation = settings.outAnimation || 'fadeOut';
        return settings;
    }

    function doTrans(settings) {
        var activeView = settings.activeView,
          newChild = settings.child,
          outAn = animValue(settings.outAnimation),
          inAn = animValue(settings.inAnimation),
          $previousView,
          $newView = $(newChild).removeClass([outAn, inAn]).addClass('animated');

        return system.defer(function (dfd) {
            if (newChild) {
                startTransition();
            } else {
                endTransistion();
            }

            function startTransition() {
                if (settings.activeView) {
                    outTransition(inTransition);
                } else {
                    inTransition();
                }
            }

            function outTransition(callback) {
                $previousView = $(activeView);
                $previousView.addClass('animated');
                $previousView.addClass(outAn);
                setTimeout(function () {
                    if (callback) {
                        callback();
                        endTransistion();
                    }
                }, App.duration);
            }
            function inTransition() {
                settings.triggerAttach();
                $newView.css('display', '');
                $newView.addClass(inAn);

                setTimeout(function () {
                    $newView.removeClass(inAn + ' ' + outAn + ' animated');
                    endTransistion();
                }, App.duration);
            }

            function endTransistion() {
                dfd.resolve();
            }
        }).promise();
    }
});