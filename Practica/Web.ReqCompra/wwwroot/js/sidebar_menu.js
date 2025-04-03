//alert();
var colap = localStorage.getItem("colaps");
if (colap == true || colap == "true") {
    $("#wrapper").toggleClass("toggled-2");
    $('#menu ul').hide();
    $('#profileId').css('display', 'none');
    $('.text-ico').css('color', 'transparent');
}


$("#menu-toggle").click(function (e) {
    e.preventDefault();
    
    //$("#hdnColaps") = false;
    localStorage.removeItem("colaps");
    localStorage.setItem("colaps", "false");
    
    $("#wrapper").toggleClass("toggled");
    if ($('#profileId').is(":visible")) {
        $('#profileId').css('display', 'none');
        $('.text-ico').css('color', 'transparent');
    } else {
        $('#profileId').css('display', 'block');
        $('.text-ico').css('color', '');
    }

});
$("#menu-toggle-2").click(function (e) {
    e.preventDefault();
    $("#wrapper").toggleClass("toggled-2");
    $('#menu ul').hide();
    //$("#hdnColaps") = true;
    if (localStorage.getItem("colaps") == "true" || localStorage.getItem("colaps") == true)
        localStorage.setItem("colaps", "false");
    else
        localStorage.setItem("colaps", "true");

    //alert(localStorage.getItem("colaps"));
    
    if ($('#profileId').is(":visible")) {
        $('#profileId').css('display', 'none');
        $('.text-ico').css('color', 'transparent');
    } else {
        $('#profileId').css('display', 'block');
        $('.text-ico').css('color', '');
    }


});



     function initMenu() {
      $('#menu ul').hide();
      $('#menu ul').children('.current').parent().show();
      //$('#menu ul:first').show();
      $('#menu li a').click(
        function() {
          var checkElement = $(this).next();
          if((checkElement.is('ul')) && (checkElement.is(':visible'))) {
            return false;
            }
          if((checkElement.is('ul')) && (!checkElement.is(':visible'))) {
            $('#menu ul:visible').slideUp('normal');
            checkElement.slideDown('normal');
            return false;
            }
          }
        );
      }
$(document).ready(function () {
    initMenu();


    
    //if ($("#hdnColaps").val()==true) {
    //    $("#wrapper").toggleClass("toggled-2");
    //    $('#menu ul').hide();
    //} 
});