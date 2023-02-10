
const btnScrolltoTop = document.querySelector(".scroll-to-top");
const btnScrollToTopStyle = btnScrolltoTop.style;

const scrollTop = () => {
    window.scrollTo({
        top: 0,
        behavior: "smooth",
    });
};
//Set default location in web and scroll smooth

const displayBtnScrollToTop = () => {
    const displayBtn = window.scrollY > 300 ? "visible" : "hidden";

    btnScrollToTopStyle.visibility = displayBtn;
};
//Check location then display btn

window.addEventListener("scroll", displayBtnScrollToTop);
btnScrolltoTop.addEventListener("click", scrollTop);
