/*$(function () {
    //showHideNav on page load
    showHideNav();
    $(window).scroll(function () {
        //show/hide nav on window's scroll
        showHideNav();

    });

    function showHideNav() {
        if ($(window).scrollTop() > 50) {

            //show white nav
            $("nav").addClass("white-nav-top");
            //show dark logo
            $(".navbar-brand img").attr("src", "img/pre-login/purple-logo.png");
            
        } else {
            //hide white nav
            $("nav").removeClass("white-nav-top");
            //show  logo
            $(".navbar-brand img").attr("src", "img/pre-login/top-logo.png");
            
            

        }

    }


});*/
/*==========================
|   |    mobile menu
==========================*/
$(function () {

    //show mobile navigation
    $("#mobile-nav-open-btn").click(function () {
        $("#mobile-nav").css("height", "100%");
    });
    //hide mobile navigation
    $("#mobile-nav-close-btn, #mobile-nav a").click(function () {
        $("#mobile-nav").css("height", "0%");
    });


});
 $(".toggle-password").click(function() {

  $(this).toggleClass("fa-eye fa-eye-slash");
  var input = $(".lo");
  if (input.attr("type") == "password") {
    input.attr("type", "text");
  } else {
    input.attr("type", "password");
  }
});


var acc = document.getElementsByClassName("accordion");
var i;

for (i = 0; i < acc.length; i++) {
  acc[i].addEventListener("click", function() {
    /* Toggle between adding and removing the "active" class,
    to highlight the button that controls the panel */
    this.classList.toggle("active");

    /* Toggle between hiding and showing the active panel */
    var panel = this.nextElementSibling;
    if (panel.style.display === "block") {
      panel.style.display = "none";
    } else {
      panel.style.display = "block";
    }
  });
}
var acc = document.getElementsByClassName("accordion");
var i;

for (i = 0; i < acc.length; i++) {
  acc[i].addEventListener("click", function() {
    this.classList.toggle("active");
    var panel = this.nextElementSibling;
    if (panel.style.maxHeight) {
      panel.style.maxHeight = null;
    } else {
      panel.style.maxHeight = panel.scrollHeight + "px";
    }
  });
}