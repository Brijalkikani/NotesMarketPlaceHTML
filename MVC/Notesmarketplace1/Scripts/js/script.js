
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
/*==========================
|   |    faq page accordian
==========================*/
 
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

/*==========================
|   |   login show/hide 
==========================*/
$(".toggle-password").click(function() {

  $(this).toggleClass("fa-eye fa-eye-slash");
  var input = $(".lo");
  if (input.attr("type") == "password") {
    input.attr("type", "text");
  } else {
    input.attr("type", "password");
  }
});
/*==========================
|   |    sign up show/hide
==========================*/
$(".toggle-password").click(function() {

  $(this).toggleClass("fa-eye fa-eye-slash");
  var input = $(".su");
  if (input.attr("type") == "password") {
    input.attr("type", "text");
  } else {
    input.attr("type", "password");
  }
});
/*==========================
|   |    check box
==========================*/
$(function () {
    
    $("input[name='IsPaid']").click(function () {
       
        if ($("#box2").is(":checked")) {
            
            $("#sellprice").removeAttr("disabled");
            $("#sellprice").focus();
        } else {
            
            if ($("#box1").is(":checked")) {
                $("#sellprice").attr("disabled", "disabled");
            }
        }
    });
});