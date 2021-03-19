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
        console.log(e);
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
        var imageType = getImageType(image);	
        var image_used = image.replace(imageType, '');	
        image_content[i] = image_used;	
    }	
    return image_content;	
}	
function getImageType(imageContent) {	
    var imageTypes = [	
        "data:image/png;base64,",	
        "data:image/jpeg;base64,",	
        "data:image/jpg;base64,"	
    ]	
    var imageType = '';	
    imageTypes.forEach(function (type) {	
        if (imageContent.includes(type)) {	
            imageType = type;	
        }	
    });	
    return imageType;	
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

function showEditModal(clickedItemName) {
    var item_name = clickedItemName.innerText; 
    try {
        $.ajax({
            url: '/Home/GetItemInformation',
            type: 'POST',
            data: {
                itemName: item_name
            },
            success: function (result) {
                addDataToEditModal(result.images, result.Item, result.ItemCount, result.Cost, result.SaleCost, result.Profit, result.SendCost, result.SendCostProfit, result.PakagingCost, result.CustomerNames);
            },
            error: function (error) {
                console.log("error");
            }
        })
    }
    catch (e) {

    }
    
}

function addDataToEditModal(images, item_name, item_count, cost, price, profit, sendcost, sendcost_profit, package_cost, customers) {
    convertImagesToSlider(images);
    addItemNameToModal(item_name)
    addItemCountToModal(item_count);
    addCostToModal(cost);
    addPriceToModal(price);
    addProfitToModal(profit);
    addSendCostToModal(sendcost);
    addSendCostProfitToModal(sendcost_profit);
    addPackageCostToModal(package_cost);
    addCustomersToModal(item_count, customers)
}


function convertImagesToSlider(images) {
    if (images == null) return;
    if (images.length < 1) {
        return '';
    }

    var container = document.getElementById('carouselInnerImages');
    var container_indicators = document.getElementById('carouselIndicators');
    
    var imageCode = 'data:image/png;base64,';
    var images_count = images.length;

    for (i = 0; i < images_count; i++) {
        var image = document.createElement('img');
        var image_indicator = document.createElement('li');
        var image_container = document.createElement('div');
        var carousel_item = document.createElement('carousel-item');

        image.classList.add("column_image");
        image.setAttribute("src", imageCode + images[i]);

        if (i == 0) {
            carousel_item.classList.add("carousel-item");
            carousel_item.classList.add("active");
        }
        else {
            carousel_item.classList.add("carousel-item");
        }
        image_container.classList.add('imag-slider');
        image_container.appendChild(image);
        carousel_item.appendChild(image_container);

        image_indicator.setAttribute('data-slide-to', i.toString());

        container.appendChild(carousel_item);
        container_indicators.appendChild(image_indicator);
    }
}

function addItemCountToModal(item_count) {
    var itemCount = document.getElementById('inputFieldItemCount_OnModal');
    itemCount.value = item_count;
}

function addCostToModal(cost) {
    var itemCost = document.getElementById('inputFieldCost_OnModal');
    itemCost.value = cost;
}

function addItemNameToModal(item_name) {
    var itemName = document.getElementById('inputFieldItemName_OnModal');
    itemName.value = item_name;
}

function addPriceToModal(price) {
    var itemPrice = document.getElementById('inputFieldSale_OnModal');
    itemPrice.value = price;
}

function addProfitToModal(profit) {
    var itemProfit = document.getElementById('inputFieldProfit_OnModal');
    itemProfit.value = profit;
}

function addSendCostToModal(sendcost) {
    var itemSendcost = document.getElementById('inputFieldSendCost_OnModal');
    itemSendcost.value = sendcost;
}

function addSendCostProfitToModal(sendcost_profit) {
    var itemSendCostProfit = document.getElementById('inputFieldSendCostProfit_OnModal');
    itemSendCostProfit.value = sendcost_profit;
}

function addPackageCostToModal(package_cost) {
    var itemPackageCost = document.getElementById('inputFieldPackageCost_OnModal');
    itemPackageCost.value = package_cost;
}

function addCustomersToModal(itemCount, customers) {
    var container = document.getElementById('inputFieldCustomerName_OnModal');
    container.innerHTML = '';
    for (i = 0; i < itemCount; i++) {
        var row_container = document.createElement('div');
        var column_container_md8 = document.createElement('div');
        var column_container_md4 = document.createElement('div');
        var inputfield_customer_name = document.createElement('input');

        row_container.classList.add('row');
        column_container_md8.classList.add('col-md-8');
        column_container_md4.classList.add('col-md-4');

        if (i < customers.length) {
            inputfield_customer_name.setAttribute('id', 'customer_name' + i);
            inputfield_customer_name.setAttribute('type', 'text');
            inputfield_customer_name.setAttribute('value', customers[i]);
            inputfield_customer_name.classList.add('form-control');
        }

        column_container_md8.appendChild(inputfield_customer_name);

        row_container.appendChild(column_container_md8);
        row_container.appendChild(column_container_md4);
        container.appendChild(row_container);
    }
}

function getImageFromDatabase() {
    $.ajax({
        url: '/Home/LoadImage',
        type: 'POST',
        data: {
            itemName: 'Test1'
        },
        success: function (result) {
            convertImagesToSlider(result);
        },
        error: function (error) {
            console.log("error");
        }
    });
}

function getImageFromID(elem) {

}

function saveChange() {
    var itemName = document.getElementById('inputFieldItemName_OnModal');
    var itemCount = document.getElementById('inputFieldItemCount_OnModal');
    var cost = document.getElementById('inputFieldCost_OnModal');
    var price = document.getElementById('inputFieldSale_OnModal');
    var profit = document.getElementById('inputFieldProfit_OnModal');
    var sendCost = document.getElementById('inputFieldSendCost_OnModal');
    var sendCostProfit = document.getElementById('inputFieldSendCostProfit_OnModal');
    var packageCost = document.getElementById('inputFieldPackageCost_OnModal');
    var customers = document.getElementById('inputFieldCustomerName_OnModal');

    var customerLength = customers.children.length;
    var customerList = []
    for (var i = 0; i < customerLength; i++) {
        customerList.push(customers.children[i].children[0].children[0].value);
    }

    try {
        $.ajax({
            url: '/Home/UpdateData',
            type: 'POST',
            data: {
                name: itemName.value,
                itemCount: itemCount.value,
                cost: cost.value,
                price: price.value,
                profit: profit.value,
                sendCost: sendCost.value,
                sendCostProfit: sendCostProfit.value,
                packagingCost: packageCost.value,
                customers: customerList
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
        console.log(e);
    }
}

$(function () {
    $("#reportDate").on("change", function () {
        var selected = $(this).val();
        summary(selected);
    });
});