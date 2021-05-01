function AddToCart(obj) {
    var soluong = $('#sst').val();
    $("#success-alert").removeClass("hiden-success");
    function add_success() {
        $('#success-alert').fadeTo(2500, 500).slideUp(500, function () {
            $('#success-alert').slideUp(500);
        });
    }

    var MaSP = obj;
    $.ajax({
        url : "/Cart/AddToCart",
        data : { MaSP : MaSP, soluong : soluong },
        dataType: "json",
        type: "POST",
        success: function (response) {
            if (!response.warning) {
                $('.count_cart').html(response.status.TongSoLuong);
                add_success();
            }
            else {
                showMess();
            }
        }
    });
    function showMess() {
        var x = document.getElementById("failed");
        x.className = "show";
        setTimeout(function () {
            x.className = x.className.replace("show", ""); }, 3000);
    };
}
$(document).ready(function () {
    $("#success-alert").addClass("hiden-success");
    $("success-alert").hide();
});