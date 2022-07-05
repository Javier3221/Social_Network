// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

let updateBtn = document.querySelectorAll(".btn-update");
let updateBlockArray = Array.from(document.querySelectorAll(".update-block"));
let i = 0;

updateBlockArray.forEach((element) => {
    updateBtn[i].addEventListener("click", () => {
        element.classList.toggle("visually-hidden");
    })
    i = i + 1;
})