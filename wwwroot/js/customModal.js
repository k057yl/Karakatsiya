document.addEventListener("DOMContentLoaded", function () {
    const openModalButtons = document.querySelectorAll(".open-modal-btn");
    const modal = document.getElementById("customModal");
    const modalBackdrop = document.getElementById("customModalBackdrop");
    const modalTitle = document.getElementById("modalTitle");
    const modalMessage = document.getElementById("modalMessage");
    const confirmActionButton = document.getElementById("confirmActionButton");
    const cancelModalButton = document.getElementById("cancelModalButton");
    const closeModalButton = document.getElementById("closeModalButton");

    let actionUrl = null;
    let itemId = null;

    document.body.addEventListener("click", function (event) {
        if (event.target.classList.contains("open-modal-btn")) {
            actionUrl = event.target.getAttribute("data-action");
            itemId = event.target.getAttribute("data-id");

            modalTitle.textContent = event.target.getAttribute("data-title");
            modalMessage.textContent = event.target.getAttribute("data-message");

            modal.style.display = "block";
            modalBackdrop.style.display = "block";
        }
    });

    function closeModal() {
        modal.style.display = "none";
        modalBackdrop.style.display = "none";
    }

    cancelModalButton.addEventListener("click", closeModal);
    closeModalButton.addEventListener("click", closeModal);
    modalBackdrop.addEventListener("click", closeModal);

    confirmActionButton.addEventListener("click", function () {
        if (actionUrl && itemId) {
            let fetchUrl = actionUrl.includes("Sale")
                ? `${actionUrl}?saleId=${itemId}`
                : `${actionUrl}?id=${itemId}`;

            console.log("Отправка запроса на:", fetchUrl);

            fetch(fetchUrl, { method: "POST" })
                .then(response => response.json())
                .then(data => {
                    console.log("Ответ сервера:", data);
                    if (data.success) {
                        console.log("Удаление успешно, закрываем окно");
                        closeModal();
                        setTimeout(() => location.reload(), 300);
                    } else {
                        alert("Ошибка: " + data.message);
                    }
                })
                .catch(error => console.error("Ошибка запроса:", error));
        }
    });
});