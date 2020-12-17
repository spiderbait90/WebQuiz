// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function funcBinary(trueAuthor, chosenAuthor, binaryAnswer) {

    $.post("/User/Quotes/AcceptAnswer", { trueAuthor: trueAuthor, chosenAuthor: chosenAuthor, binaryAnswer: binaryAnswer });

    var correct = "You answered correctly!";
    var error = "Your answer is wrong! Correct answer is: " + trueAuthor;

    if (trueAuthor === chosenAuthor && binaryAnswer === "yes") {
        alert(correct);
    } else if (trueAuthor !== chosenAuthor && binaryAnswer === "yes") {
        alert(error);
    }
    else if (trueAuthor === chosenAuthor && binaryAnswer === "no") {
        alert(error);
    }
    else if (trueAuthor !== chosenAuthor && binaryAnswer === "no") {
        alert(correct);
    }

    $("#BtnToHide").show();
    $("#binaryBtn1").hide();
    $("#binaryBtn2").hide();
    $("#AuthorOption").hide();
    $("#Author").show();
}

function funcMulti(trueAuthor, chosenAuthor) {

    $.post("/User/Quotes/AcceptAnswer", { trueAuthor: trueAuthor, chosenAuthor: chosenAuthor });

    var correct = "You answered correctly!";
    var error = "Your answer is wrong! Correct answer is: " + trueAuthor;

    if (trueAuthor === chosenAuthor)
        alert(correct);
    else {
        alert(error);
    }

    $("#BtnToHide").show();
    $("#Author").show();
    $("#btnMulti1").hide();
    $("#btnMulti2").hide();
    $("#btnMulti3").hide();
}
