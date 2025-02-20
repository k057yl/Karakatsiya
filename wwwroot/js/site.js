document.addEventListener("DOMContentLoaded", function () {
    const fileInput = document.getElementById("fileUpload");
    const thumbnail = document.getElementById("thumbnail");
    const removeButton = document.getElementById("removeButton");
    const fileLabel = document.querySelector("label[for='fileUpload']");
    const defaultLabelText = "Select File";

    if (fileInput && thumbnail && removeButton) {
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

    const showExpirationDateCheckbox = document.getElementById("ShowExpirationDateCheckbox");
    const expirationDateContainer = document.getElementById("expirationDateContainer");

    if (showExpirationDateCheckbox && expirationDateContainer) {
        showExpirationDateCheckbox.addEventListener("change", function () {
            expirationDateContainer.style.display = showExpirationDateCheckbox.checked ? "block" : "none";
        });
    }

    // Modal setup for deleting items
    const deleteModal = document.getElementById("deleteModal");
    if (deleteModal) {
        deleteModal.addEventListener("show.bs.modal", function (event) {
            const button = event.relatedTarget;
            const itemId = button.getAttribute("data-itemid");
            const formAction = `/Item/Delete/${itemId}`;
            const deleteForm = document.getElementById("deleteForm");
            deleteForm.setAttribute("action", formAction);
        });
    }

    // Sell item functionality
    document.querySelectorAll("form[action='SellItem']").forEach(form => {
        form.addEventListener("submit", function (event) {
            event.preventDefault();

            const itemId = form.querySelector("input[name='itemId']").value;
            const salePrice = form.querySelector("input[name='salePrice']").value;
            const profit = form.querySelector("input[name='profit']").value;

            if (!salePrice || parseFloat(salePrice) <= 0) {
                alert("Please enter valid sale price and profit.");
                return;
            }

            fetch("/Sale/Sales", {
                method: "POST",
                headers: { "Content-Type": "application/x-www-form-urlencoded" },
                body: new URLSearchParams({
                    itemId: itemId,
                    salePrice: salePrice,
                    profit: profit
                })
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

    // Delete sell functionality
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
        fetch(`/Sale/DeleteSale?saleId=${saleId}`, {
            method: 'POST'
        }).then(() => location.reload());
    }

    document.addEventListener("hidden.bs.modal", function () {
        const backdrops = document.querySelectorAll(".modal-backdrop");
        backdrops.forEach(backdrop => backdrop.remove());
    });
});