(function( $ ){
	"use strict";
	$.fn.wysiwyg = function( options ){
		var opts = $.extend( {}, $.fn.wysiwyg.defaults, options ), 
			i = 0, 
			j = 0, 
			sizeMax = 8, 
			textarea = this,
			iframe = $( "<iframe style='clear:both;display:block;' height='500px' name='richtext' id="+textarea[0].id+"richtext></iframe>" );


		iframe.insertAfter( textarea );
		var idoc = iframe[0].contentDocument || iframe[0].contentWindow.document, // ie compatibility
		styles = "<style>"+
						".dropdown {position: relative;display: inline-block;width: 19.5px;}"+
						".dropdown-contents {display: none;position: absolute;background-color: #f9f9f9;min-width: 32px;box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2);padding: 12px 16px;}"+
						".dropdown:hover .dropdown-contents {display: block;}"+
						".color{width: 20px!important;height:20px!important;cursor:pointer;}"+
						"/* Modal full page box */"+
						".custommodal {display: none; position: fixed; z-index: 10; left: 0;top: 0;width: 100%; height: 100%; overflow: auto; background-color: rgb(0,0,0); background-color: rgba(0,0,0,0.4);}"+
						"/* Modal Content/Box */"+
						".modal-content {background-color: #fefefe;margin: 15% auto; padding: 20px;border: 1px solid #888;width: 80%; }"+
						"/* Modal close button */"+
						".close {color: #aaa;float: right;font-size: 28px;font-weight: bold;}"+
						".close:hover,.close:focus {color: black;text-decoration: none;cursor: pointer;}"+
					"</style>";
		var codeInjection = "<div style='display:inline-block;'>"+
								styles;
		if ( opts.allowBasic ) {
			codeInjection+=	"	<a data-command='bold' class='waves-effect waves-light btn'><i class='material-icons'>format_bold</i></a>"+
							"	<a data-command='italic' class='waves-effect waves-light btn'><i class='material-icons'>format_italic</i></a>"+
							"	<a data-command='underline' class='waves-effect waves-light btn'><i class='material-icons'>format_underline</i></a>"+
							"	<a data-command='strikeThrough' class='waves-effect waves-light btn'><i class='material-icons'>format_strikethrough</i></a>";
		}
		if ( opts.allowPositioning ) {
			codeInjection +="	<a data-command='justifyLeft' class='waves-effect waves-light btn'><i class='material-icons'>format_align_left</i></a>"+
							"	<a data-command='justifyCenter' class='waves-effect waves-light btn'><i class='material-icons'>format_align_center</i></a>"+
							"	<a data-command='justifyRight' class='waves-effect waves-light btn'><i class='material-icons'>format_align_right</i></a>"+
							"	<a data-command='justifyFull' class='waves-effect waves-light btn'><i class='material-icons'>format_align_justify</i></a>";
		}
		if ( opts.allowLists ) {
			codeInjection+=	"	<a data-command='insertUnorderedList' class='waves-effect waves-light btn'><i class='material-icons'>format_list_bulleted</i></a>"+
							"	<a data-command='insertOrderedlist' class='waves-effect waves-light btn'><i class='material-icons'>format_list_numbered</i></a>";
		}
		if ( opts.allowImages ) {
			codeInjection+=	"	<a data-modal='image' class='waves-effect waves-light btn'><i class='material-icons'>insert_photo</i></a>"+
							"	<div class='custommodal' id='image'>"+
							"		<div class='modal-content'>"+
							"			<span class='close'>x</span>"+
							"			<input type='text' placeholder='imagelink'>"+
							"			<p>Type an url to an image in the input above</p>"+
							"			<a data-command='insertImageCustom' class='waves-effect waves-light btn'>Insert image</a>"+
							"		</div>"+
							"	</div>";
		}
		if ( opts.allowLinks ) {
			codeInjection+=	"	<a data-modal='link' class='waves-effect waves-light btn'><i class='material-icons'>insert_link</i></a>"+
							"	<div class='custommodal' id='link'>"+
							"		<div class='modal-content'>"+
							"			<span class='close'>x</span>"+
							"			<input type='text' placeholder='sitename'>"+
							"			<p>Type the site name in the input above</p>"+
							"			<a data-command='createLink' class='waves-effect waves-light btn'>Insert link</a>"+
							"		</div>"+
							"	</div>";
		}
		if ( opts.allowColor ) {
			codeInjection+= "	<div class='dropdown'>"+
							"		<i class='material-icons'>format_color_text</i>"+
							"		<div class='dropdown-contents'>";
			for ( ; i < opts.colors.length; i++ ) {
				codeInjection += "			<div data-command='foreColor' data-value='"+opts.colors[i]+"' class='color' style='background-color:"+opts.colors[i]+"'></div>";
			}
			codeInjection+=	"		</div>"+
							"	</div>";
		}

		if ( opts.allowSize ) {
			codeInjection+=	"	<div class='dropdown'><i class='material-icons'>format_size</i>"+
							"		<div class='dropdown-contents'>";
			for ( ; j < sizeMax; j++ ) {
				codeInjection+="			<div style='cursor:pointer' data-command='fontSize' data-value='"+j+"'>"+j+"</div><br>";
			}			
			codeInjection+=	"		</div>"+
							"	</div>";
		}
		codeInjection+=		"	<a data-modal='fontSize' class='waves-effect waves-light btn'><i class='material-icons'>format_size</i></a>"+
							"	<div class='custommodal' id='fontSize'>"+
							"		<div class='modal-content'>"+
							"			<span class='close'>x</span>"+
							"			<input type='text' placeholder='px size'>"+
							"			<p>Type the pixel size in the input above</p>"+
							"			<a data-custom-command='fontSize' class='waves-effect waves-light btn'>font size</a>"+
							"		</div>"+
							"	</div>";


		codeInjection += 	"	<a data-command='removeFormat' class='waves-effect waves-light btn'><i class='material-icons'>format_clear</i></a>"+
							"	<a data-command='showCode' class='waves-effect waves-light btn'><i class='material-icons'>code</i></a>"+
							"</div>";

		var buttons = $( codeInjection );
		buttons.insertBefore( textarea );

		//textarea.css('display','none');
		idoc.designMode = "On";
		var width = buttons.css( "width" );
		iframe.css( "width", width );

		// idoc.getSelection(); to check if user selected something -- for custom code
		// When text selected: 		Selection {anchorNode: textanchorOffset: 12baseNode: textbaseOffset: 12extentNode: textextentOffset: 0focusNode: textfocusOffset: 0isCollapsed: falserangeCount: 1type: "Range"__proto__: Selection}
		// When no text selected: 	Selection {anchorNode: divanchorOffset:0baseNode:divbaseOffset:0extentNode:divextentOffset:0focusNode:divfocusOffset:0isCollapsed:truerangeCount:1type:"Caret"}

		$( textarea ).parent().on("click", "a", function(){
			if ( $( this ).attr( "data-command" ) !== undefined ) {
				if ( $( this ).attr( "data-command" ) === "insertImageCustom" ) {
					var image = $( this ).parent().find( "input" ).val();
					idoc.execCommand( $( this ).attr( "data-command" ).splice( -6 ), false, image );
					$( this ).parent().parent().css( "display","none" );
					$( this ).parent().find( "input" ).val( "" );
				} else if ( $(this).attr("data-command") === "createLink" ) {
					var site = $( this ).parent().find( "input" ).val();
					idoc.execCommand( $( this ).attr( "data-command" ), false, site );
					$( this ).parent().parent().css( "display","none" );
					$( this ).parent().find( "input" ).val( "" );
				} else if ( $(this).attr("data-command") === "insertImage" ) {
					idoc.execCommand( $( this ).attr( "data-command" ), false, $( this ).attr( "data-value" ) );
					$( this ).parent().parent().css( "display","none" );
					idoc.execCommand( "enableObjectResizing", true, true );
				} else if ( $(this).attr("data-command") === "showCode" ) {
					$( textarea ).val( idoc.body.innerHTML );
				} else {
					idoc.execCommand( $( this ).attr( "data-command" ), false, $( this ).attr( "data-value" ) );
				}
				$( iframe ).focus();
				$( textarea ).val( idoc.body.innerHTML );
			} else if ( $( this ).attr( "data-custom-command" ) !== undefined ) {
				var selection = idoc.getSelection();
				console.log( selection );
				if ( $( this ).attr( "data-custom-command" ) === "fontSize" ) {
					var size = $( this ).parent().find( "input" ).val(),
						range = selection.getRangeAt( 0 ),
						content = range.extractContents(),
						span = document.createElement( "SPAN" );
					span.appendChild( content );
					span.style.fontSize = size + "px";
					range.insertNode(span);
					$( this ).parent().parent().css( "display","none" );
					$( this ).parent().find( "input" ).val( "" );
				}
			} else {
				if ( $( this ).attr( "data-modal" ) === "image" && opts.imageAjax ) {
					var that = this;
					$.ajax({
						method: opts.imageAjax.method,
						url: opts.imageAjax.url
					})
					.done(function( data ){
						var i = 0,
							alreadyLoaded = false,
							images = $( that ).next().children( "div" ).children( "img" ),
							j = 0;			
						for (; i < data.length; i++) {
							for ( ; j < images.length; j++ ) {
								if ( $( images[j] ).attr( "src" ) == data[i].url ) {
									alreadyLoaded = true;
								}
							}
							if ( !alreadyLoaded ) {
								$( that ).next().find( "div.modal-content" ).append( 
									"<br><img src='"+data[i].url+
									"' width='50px' height='50px'><a data-command='insertImage' data-value='"+data[i].url+
									"' class='waves-effect waves-light btn'>"+data[i].name+
									"</a>" 
								);
							}
							alreadyLoaded = false;
							j = 0;
						}
					});
				}
				$( this ).next().css( "display", "block" );
				$( textarea ).val( idoc.body.innerHTML );
			}
		});
		$( textarea ).parent().on("click", "div", function(){
			if ( $( this ).attr( "data-command" ) !== undefined ) {
				idoc.execCommand( $( this ).attr( "data-command" ), false, $( this ).attr( "data-value" ) );
				$( iframe ).focus();
			}
			$( textarea ).val( idoc.body.innerHTML );
		});
		$( textarea ).parent().on("click", "span", function(){
			$( this ).parent().parent().css( "display","none" );
			$( textarea ).val( idoc.body.innerHTML );
		});

		$( textarea ).on("change", function(){
			var code = $( this ).val();
			idoc.body.innerHTML = code;
		});

		$( idoc ).on("change keyup", function(){
			var design = idoc.body.innerHTML;
			$( textarea ).val( design );
		});

		function parentUntilBody( el, list, callback ){z
			if( $( el )[0].nodeName !== "BODY" ) {
				if ( $( el )[0].nodeName === "SPAN" ) {
					var style = $( el )[0].nodeName.style;
					for ( var child in style ) {
						if ( ( !style[child] ) || child !== "length" ) {
							list.push( $( el )[0].nodeName+"_"+style[child] );
						}
					}
				} else {
					list.push( $( el )[0].nodeName );
				}
				parentUntilBody( el.parentNode, list, callback );
			} else {
				callback( list );
			}
		}

		$( idoc ).on("click", function(){
			var selection = idoc.getSelection();
			console.log( selection );
			parentUntilBody( selection.anchorNode, [], function( data ){
				console.log( data );
			});
		});
		return textarea;
	};

	$.fn.wysiwyg.defaults = {
		allowSize: true,
		allowColor: true,
		colors: ["black","white","red","blue","green"],
		allowPositioning: true,
		allowBasic:true,
		allowLists:true,
		allowLinks:true,
		allowImages:true,
		imageAjax: false
	};
}( jQuery ));