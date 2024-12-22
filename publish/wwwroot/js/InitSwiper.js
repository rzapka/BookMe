for (let i = 1; i <= 2; i++) {
    var swiper = new Swiper(`.swiper${i}`, {
        slidesPerView: getSlidesPerView(),
        spaceBetween: 30,
        direction: getDirection(),
        navigation: {
            nextEl: `.swiper-button-next${i}`,
            prevEl: `.swiper-button-prev${i}`,
        },
        on: {
            resize: function () {
                swiper.changeDirection(getDirection());
                swiper.params.slidesPerView = getSlidesPerView();
                swiper.update();
            },
        },
        breakpoints: {
            640: {
                slidesPerView: 1,
                spaceBetween: 20,
            },
            768: {
                slidesPerView: 2,
                spaceBetween: 30,
            },
            1024: {
                slidesPerView: 3,
                spaceBetween: 30,
            },
        },
    });
}

function getDirection() {
    return window.innerWidth <= 760 ? 'vertical' : 'horizontal';
}

function getSlidesPerView() {
    if (window.innerWidth < 640) {
        return 1;
    } else if (window.innerWidth < 768) {
        return 2;
    } else if (window.innerWidth < 1024) {
        return 3;
    } else {
        return 5; 
    }
}
