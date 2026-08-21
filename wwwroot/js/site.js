document.addEventListener("DOMContentLoaded", () => {
    setTimeout(() => {
        document.querySelectorAll(".alert.success").forEach(el => {
            el.style.opacity = "0";
            el.style.transition = "opacity .4s";
            setTimeout(() => el.remove(), 450);
        });
    }, 3500);
});
