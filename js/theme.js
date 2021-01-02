// JavaScript Document

var bs = $('#banner-carousel');
bs.owlCarousel({
	autoplay:true,
	//autoplayTimeout:1000,
	//autoplaySpeed:700,
	smartSpeed:200,
    loop:true,
    nav:true,
	dots:true,
	margin: 1,
	//animateOut: 'fadeOut',
    items: 1,
	navText: [ '<img src="images/previous.png">', '<img src="images/next.png">' ],	
	
});




