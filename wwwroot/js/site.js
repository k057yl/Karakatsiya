document.addEventListener("DOMContentLoaded", function () {
    initFileUploadPreview();
    initExpirationDateToggle();
    initDeleteModal();
    initSellFormHandling();
    initSaleDeleteHandling();
    fixBackdropIssue();
});

// Превью изображения при выборе файла
function initFileUploadPreview() {
    const fileInput = document.getElementById("fileUpload");
    const thumbnail = document.getElementById("thumbnail");
    const removeButton = document.getElementById("removeButton");
    const fileLabel = document.querySelector("label[for='fileUpload']");
    const defaultLabelText = "Select File";

    if (!fileInput || !thumbnail || !removeButton) return;

    fileInput.addEventListener("change", function (event) {
        const file = event.target.files[0];
        if (file && file.type.startsWith("image/")) {
            fileLabel.textContent = file.name;
            const reader = new FileReader();
            reader.onload = function (e) {
                thumbnail.src = e.target.result;
                thumbnail.style.display = "block";
                removeButton.style.display = "block";
            };
            reader.readAsDataURL(file);
        } else {
            clearThumbnail();
        }
    });

    removeButton.addEventListener("click", function () {
        clearThumbnail();
        fileInput.value = "";
        fileLabel.textContent = defaultLabelText;
    });

    function clearThumbnail() {
        thumbnail.style.display = "none";
        thumbnail.src = "#";
        removeButton.style.display = "none";
        fileLabel.textContent = defaultLabelText;
    }
}

// Переключатель отображения даты истечения
function initExpirationDateToggle() {
    const showExpirationDateCheckbox = document.getElementById("ShowExpirationDateCheckbox");
    const expirationDateContainer = document.getElementById("expirationDateContainer");

    if (!showExpirationDateCheckbox || !expirationDateContainer) return;

    showExpirationDateCheckbox.addEventListener("change", function () {
        expirationDateContainer.style.display = showExpirationDateCheckbox.checked ? "block" : "none";
    });
}

// Установка действия формы удаления
function initDeleteModal() {
    const deleteModal = document.getElementById("deleteModal");
    if (!deleteModal) return;

    deleteModal.addEventListener("show.bs.modal", function (event) {
        const button = event.relatedTarget;
        const itemId = button.getAttribute("data-itemid");
        const formAction = `/Item/Delete/${itemId}`;
        const deleteForm = document.getElementById("deleteForm");
        if (deleteForm) deleteForm.setAttribute("action", formAction);
    });
}

// Обработка формы продажи
function initSellFormHandling() {
    document.querySelectorAll("form[action='SellItem']").forEach(form => {
        form.addEventListener("submit", function (event) {
            event.preventDefault();

            const itemId = form.querySelector("input[name='itemId']").value;
            const salePrice = form.querySelector("input[name='salePrice']").value;
            const profit = form.querySelector("input[name='profit']").value;

            if (!salePrice || parseFloat(salePrice) <= 0 || !profit || isNaN(parseFloat(profit))) {
                alert("Please enter valid sale price and profit.");
                return;
            }

            fetch("/Sale/Sales", {
                method: "POST",
                headers: { "Content-Type": "application/x-www-form-urlencoded" },
                body: new URLSearchParams({ itemId, salePrice, profit })
            })
                .then(response => {
                    if (response.ok) {
                        location.reload();
                    } else {
                        alert("Error selling item. Please try again.");
                    }
                });
        });
    });
}

// Удаление продажи
function initSaleDeleteHandling() {
    const deleteButtons = document.querySelectorAll("button[data-saleid]");
    let saleIdToDelete = null;

    deleteButtons.forEach(button => {
        button.addEventListener("click", function () {
            saleIdToDelete = button.getAttribute("data-saleid");
            const modal = new bootstrap.Modal(document.getElementById("confirmDeleteModal"));
            modal.show();
        });
    });

    document.getElementById("confirmDeleteButton")?.addEventListener("click", function () {
        if (saleIdToDelete) {
            deleteSale(saleIdToDelete);
            saleIdToDelete = null;
        }
    });

    function deleteSale(saleId) {
        fetch(`/Sale/DeleteSale?saleId=${saleId}`, { method: "POST" })
            .then(() => location.reload());
    }
}

// Удаление лишних затемнений от модальных окон
function fixBackdropIssue() {
    document.addEventListener("hidden.bs.modal", function () {
        document.querySelectorAll(".modal-backdrop").forEach(backdrop => backdrop.remove());
    });
}