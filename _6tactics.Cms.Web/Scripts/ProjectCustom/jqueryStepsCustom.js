function jquerStepsGotoStep(step) {
    $('#wizard').steps("setStep", step);
}

function jquerStepsHeightRefrash(elementHeight)
{
    $('#wizard .content').animate({
        height: elementHeight
    }, 100);
}