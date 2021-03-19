var uploadedFiles = 0;
var itemCount = 1;
var images_list = []

$(document).ready(function () {
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); 
    var yyyy = today.getFullYear();

    var day = yyyy + '-' + mm + '-' + dd;
    getSummaryData(day);

    var $slider = $('#carousel'),
        hammer = new Hammer($slider.get(0));
    $slider.find('img').each((index, elem) => {
        $(elem).prop('draggable', false);
    });
    $slider.carousel();
    $slider.find(".carousel-control-prev").click(e => {
        e.preventDefault();
        $slider.carousel("prev");
    });
    $slider.find(".carousel-control-next").click(e => {
        e.preventDefault();
        $slider.carousel("next");
    });
    hammer.on("panleft panright", e => {
        e.preventDefault();

        if (e.type == 'panleft') {
            $slider.carousel("next");
        }

        if (e.type == 'panright') {
            $slider.carousel("prev");
        }
    });
    $slider.find('.carousel-indicators li').click(e => {
        $slider.carousel($(e.target).data('slide-to'));
    });
});


function submitForm() {
    var itemName = document.getElementById("inputFieldItemName").value;
    var itemCount = document.getElementById("inputFieldItemCount").value;
    var cost = document.getElementById("inputFieldCost").value;
    var totalCost = parseInt(itemCount) * parseInt(cost);
    var images_content = getImageContent();

    try {
        $.ajax({
            url: '/Home/SubmitData',
            type: 'POST',
            data: {
                itemName: itemName,
                cost: totalCost.toString(),
                itemCount: itemCount,
                imagesData: images_content
            },
            success: function (result) {
                saveImages();
            },
            error: function (error) {
                console.log("error");
            }
        })
            .then(
                $.ajax({
                    url: '/Home',
                    type: 'GET'
                })
            )
    }
    catch (e) {

    }
}


function profitCal(cost, salePrice) {
    var tmp_cost = parseInt(cost);
    var tmp_salePrice = parseInt(salePrice);

    var profit = tmp_salePrice - tmp_cost;

    return profit;
}

function sendCostCal(sendCost) {
    return 50 - parseInt(sendCost);
}

function summary(day) {
    $.ajax({
        url: '/Home/LoadDataInADay',
        type: 'GET',
        data: {
            day: day
        },
        success: function (result) {
            $('#tbStockItemList').find('tbody').empty();
            $.each(result, function (i, v) {
                $('#tbStockItemList').find('tbody')
                    .append('<tr>')
                    .append('<td class="text_center">' + v.Item + '</td>')
                    .append('<td class="text_center">' + v.ItemCount + '</td>')
                    .append('<td class="text_center">' + v.Cost + '</td>')
                    .append('<td class="text_center">' + v.SaleCost + '</td>')
                    .append('<td class="text_center">' + v.Profit + '</td>')
                    .append('<td class="text_center">' + v.SendCost + '</td>')
                    .append('<td class="text_center">' + v.SendCostProfit + '</td>')
                    .append('<td class="text_center">' + v.PakagingCost + '</td>')
                    .append('<td class="text_center">' + v.CustomerName + '</td>')
                    .append('</tr>');
            })
        },
        error: function (error) {
            console.log("error");
        }
    })
        .then(
            getSummaryData(day)
        );
}

function getSummaryData(day) {
    $.ajax({
        url: '/Home/Summary',
        type: 'GET',
        data: {
            day: day
        },
        success: function (result) {
            summaryUpdate(result);
        },
        error: function (error) {
            console.log("error");
        }
    });
}

function summaryUpdate(data) {
    var incomeDay = document.getElementById('incomeDay');
    var costDay = document.getElementById('costDay');
    var profitDay = document.getElementById('profitDay');
    var incomeMonth = document.getElementById('incomeMonth');
    var costMonth = document.getElementById('costMonth');
    var profitMonth = document.getElementById('profitMonth');
    var itemCount = document.getElementById('itemCount');

    costDay.innerHTML = data[0];
    incomeDay.innerHTML = data[1];
    profitDay.innerHTML = data[2];
    costMonth.innerHTML = data[3];
    incomeMonth.innerHTML = data[4];
    profitMonth.innerHTML = data[5];
    itemCount.innerHTML = data[6];
}

function uploadFile(input) {
    var element = document.getElementById("gallery");
    var file_count = input.files.length;
    var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.jpg|.jpeg|.gif|.png|.bmp)$/;
    for (var i = 0; i < file_count; i++) {
        var file = input.files[i];
        if (regex.test(file.name.toLowerCase())){
            if (input.files && input.files[i]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    var upload_file = document.createElement("img");
                    upload_file.setAttribute("id", "upload_file" + uploadedFiles);
                    upload_file.classList.add("column_image");
                    upload_file.src = e.target.result;
                    element.appendChild(upload_file);
                    uploadedFiles++;
                }
                reader.readAsDataURL(input.files[i]);
            }
            
        }
    }
}

function countItemDown() {
    if (itemCount > 0) {
        itemCount = itemCount - 1;
        $("#inputFieldItemCount")[0].value = itemCount;
    }
}

function countItemUp() {
    itemCount = itemCount + 1;
    $("#inputFieldItemCount")[0].value = itemCount;
}

function getImageContent() {
    var images_count = document.getElementById("gallery").childElementCount;
    var image_content = [];

    for (var i = 0; i < images_count; i++) {
        var images_id = "upload_file" + i;
        var image = document.getElementById(images_id).src;
        var image_used = image.replace('data:image/png;base64,', '');

        image_content[i] = image_used;
    }
    return image_content;
}

function searchBags() {
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("tbSearchBox");
    filter = input.value.toUpperCase();
    table = document.getElementById("tbStockItemList");
    tr = table.getElementsByTagName("tr");

    // Loop through all table rows, and hide those who don't match the search query
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[0];
        if (td) {
            txtValue = td.textContent || td.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}

$(function () {
    $("#reportDate").on("change", function () {
        var selected = $(this).val();
        summary(selected);
    });
});