(self.webpackChunklocal_cloud=self.webpackChunklocal_cloud||[]).push([[532],{102:t=>{"use strict";var i=Object.getOwnPropertySymbols,e=Object.prototype.hasOwnProperty,n=Object.prototype.propertyIsEnumerable;function s(t){if(null==t)throw new TypeError("Object.assign cannot be called with null or undefined");return Object(t)}t.exports=function(){try{if(!Object.assign)return!1;var t=new String("abc");if(t[5]="de","5"===Object.getOwnPropertyNames(t)[0])return!1;for(var i={},e=0;e<10;e++)i["_"+String.fromCharCode(e)]=e;if("0123456789"!==Object.getOwnPropertyNames(i).map(function(t){return i[t]}).join(""))return!1;var n={};return"abcdefghijklmnopqrst".split("").forEach(function(t){n[t]=t}),"abcdefghijklmnopqrst"===Object.keys(Object.assign({},n)).join("")}catch(s){return!1}}()?Object.assign:function(t,o){for(var r,a,h=s(t),l=1;l<arguments.length;l++){for(var d in r=Object(arguments[l]))e.call(r,d)&&(h[d]=r[d]);if(i){a=i(r);for(var m=0;m<a.length;m++)n.call(r,a[m])&&(h[a[m]]=r[a[m]])}}return h}},2492:(t,i,e)=>{const n=e(6712),s=e(102),o={propertyCache:{},vendors:[null,["-webkit-","webkit"],["-moz-","Moz"],["-o-","O"],["-ms-","ms"]],clamp:(t,i,e)=>i<e?t<i?i:t>e?e:t:t<e?e:t>i?i:t,data:(t,i)=>o.deserialize(t.getAttribute("data-"+i)),deserialize:t=>"true"===t||"false"!==t&&("null"===t?null:!isNaN(parseFloat(t))&&isFinite(t)?parseFloat(t):t),camelCase:t=>t.replace(/-+(.)?/g,(t,i)=>i?i.toUpperCase():""),accelerate(t){o.css(t,"transform","translate3d(0,0,0) rotate(0.0001deg)"),o.css(t,"transform-style","preserve-3d"),o.css(t,"backface-visibility","hidden")},transformSupport(t){let i=document.createElement("div"),e=!1,n=null,s=!1,r=null,a=null;for(let h=0,l=o.vendors.length;h<l;h++)if(null!==o.vendors[h]?(r=o.vendors[h][0]+"transform",a=o.vendors[h][1]+"Transform"):(r="transform",a="transform"),void 0!==i.style[a]){e=!0;break}switch(t){case"2D":s=e;break;case"3D":if(e){let t=document.body||document.createElement("body"),e=document.documentElement,o=e.style.overflow,h=!1;document.body||(h=!0,e.style.overflow="hidden",e.appendChild(t),t.style.overflow="hidden",t.style.background=""),t.appendChild(i),i.style[a]="translate3d(1px,1px,1px)",n=window.getComputedStyle(i).getPropertyValue(r),s=void 0!==n&&n.length>0&&"none"!==n,e.style.overflow=o,t.removeChild(i),h&&(t.removeAttribute("style"),t.parentNode.removeChild(t))}}return s},css(t,i,e){let n=o.propertyCache[i];if(!n)for(let s=0,r=o.vendors.length;s<r;s++)if(n=null!==o.vendors[s]?o.camelCase(o.vendors[s][1]+"-"+i):i,void 0!==t.style[n]){o.propertyCache[i]=n;break}t.style[n]=e}},r={relativeInput:!1,clipRelativeInput:!1,inputElement:null,hoverOnly:!1,calibrationThreshold:100,calibrationDelay:500,supportDelay:500,calibrateX:!1,calibrateY:!0,invertX:!0,invertY:!0,limitX:!1,limitY:!1,scalarX:10,scalarY:10,frictionX:.1,frictionY:.1,originX:.5,originY:.5,pointerEvents:!1,precision:1,onReady:null,selector:null};t.exports=class{constructor(t,i){this.element=t;const e={calibrateX:o.data(this.element,"calibrate-x"),calibrateY:o.data(this.element,"calibrate-y"),invertX:o.data(this.element,"invert-x"),invertY:o.data(this.element,"invert-y"),limitX:o.data(this.element,"limit-x"),limitY:o.data(this.element,"limit-y"),scalarX:o.data(this.element,"scalar-x"),scalarY:o.data(this.element,"scalar-y"),frictionX:o.data(this.element,"friction-x"),frictionY:o.data(this.element,"friction-y"),originX:o.data(this.element,"origin-x"),originY:o.data(this.element,"origin-y"),pointerEvents:o.data(this.element,"pointer-events"),precision:o.data(this.element,"precision"),relativeInput:o.data(this.element,"relative-input"),clipRelativeInput:o.data(this.element,"clip-relative-input"),hoverOnly:o.data(this.element,"hover-only"),inputElement:document.querySelector(o.data(this.element,"input-element")),selector:o.data(this.element,"selector")};for(let n in e)null===e[n]&&delete e[n];s(this,r,e,i),this.inputElement||(this.inputElement=this.element),this.calibrationTimer=null,this.calibrationFlag=!0,this.enabled=!1,this.depthsX=[],this.depthsY=[],this.raf=null,this.bounds=null,this.elementPositionX=0,this.elementPositionY=0,this.elementWidth=0,this.elementHeight=0,this.elementCenterX=0,this.elementCenterY=0,this.elementRangeX=0,this.elementRangeY=0,this.calibrationX=0,this.calibrationY=0,this.inputX=0,this.inputY=0,this.motionX=0,this.motionY=0,this.velocityX=0,this.velocityY=0,this.onMouseMove=this.onMouseMove.bind(this),this.onDeviceOrientation=this.onDeviceOrientation.bind(this),this.onDeviceMotion=this.onDeviceMotion.bind(this),this.onOrientationTimer=this.onOrientationTimer.bind(this),this.onMotionTimer=this.onMotionTimer.bind(this),this.onCalibrationTimer=this.onCalibrationTimer.bind(this),this.onAnimationFrame=this.onAnimationFrame.bind(this),this.onWindowResize=this.onWindowResize.bind(this),this.windowWidth=null,this.windowHeight=null,this.windowCenterX=null,this.windowCenterY=null,this.windowRadiusX=null,this.windowRadiusY=null,this.portrait=!1,this.desktop=!navigator.userAgent.match(/(iPhone|iPod|iPad|Android|BlackBerry|BB10|mobi|tablet|opera mini|nexus 7)/i),this.motionSupport=!!window.DeviceMotionEvent&&!this.desktop,this.orientationSupport=!!window.DeviceOrientationEvent&&!this.desktop,this.orientationStatus=0,this.motionStatus=0,this.initialise()}initialise(){void 0===this.transform2DSupport&&(this.transform2DSupport=o.transformSupport("2D"),this.transform3DSupport=o.transformSupport("3D")),this.transform3DSupport&&o.accelerate(this.element),"static"===window.getComputedStyle(this.element).getPropertyValue("position")&&(this.element.style.position="relative"),this.pointerEvents||(this.element.style.pointerEvents="none"),this.updateLayers(),this.updateDimensions(),this.enable(),this.queueCalibration(this.calibrationDelay)}doReadyCallback(){this.onReady&&this.onReady()}updateLayers(){this.layers=this.selector?this.element.querySelectorAll(this.selector):this.element.children,this.layers.length||console.warn("ParallaxJS: Your scene does not have any layers."),this.depthsX=[],this.depthsY=[];for(let t=0;t<this.layers.length;t++){let i=this.layers[t];this.transform3DSupport&&o.accelerate(i),i.style.position=t?"absolute":"relative",i.style.display="block",i.style.left=0,i.style.top=0;let e=o.data(i,"depth")||0;this.depthsX.push(o.data(i,"depth-x")||e),this.depthsY.push(o.data(i,"depth-y")||e)}}updateDimensions(){this.windowWidth=window.innerWidth,this.windowHeight=window.innerHeight,this.windowCenterX=this.windowWidth*this.originX,this.windowCenterY=this.windowHeight*this.originY,this.windowRadiusX=Math.max(this.windowCenterX,this.windowWidth-this.windowCenterX),this.windowRadiusY=Math.max(this.windowCenterY,this.windowHeight-this.windowCenterY)}updateBounds(){this.bounds=this.inputElement.getBoundingClientRect(),this.elementPositionX=this.bounds.left,this.elementPositionY=this.bounds.top,this.elementWidth=this.bounds.width,this.elementHeight=this.bounds.height,this.elementCenterX=this.elementWidth*this.originX,this.elementCenterY=this.elementHeight*this.originY,this.elementRangeX=Math.max(this.elementCenterX,this.elementWidth-this.elementCenterX),this.elementRangeY=Math.max(this.elementCenterY,this.elementHeight-this.elementCenterY)}queueCalibration(t){clearTimeout(this.calibrationTimer),this.calibrationTimer=setTimeout(this.onCalibrationTimer,t)}enable(){this.enabled||(this.enabled=!0,this.orientationSupport?(this.portrait=!1,window.addEventListener("deviceorientation",this.onDeviceOrientation),this.detectionTimer=setTimeout(this.onOrientationTimer,this.supportDelay)):this.motionSupport?(this.portrait=!1,window.addEventListener("devicemotion",this.onDeviceMotion),this.detectionTimer=setTimeout(this.onMotionTimer,this.supportDelay)):(this.calibrationX=0,this.calibrationY=0,this.portrait=!1,window.addEventListener("mousemove",this.onMouseMove),this.doReadyCallback()),window.addEventListener("resize",this.onWindowResize),this.raf=n(this.onAnimationFrame))}disable(){this.enabled&&(this.enabled=!1,this.orientationSupport?window.removeEventListener("deviceorientation",this.onDeviceOrientation):this.motionSupport?window.removeEventListener("devicemotion",this.onDeviceMotion):window.removeEventListener("mousemove",this.onMouseMove),window.removeEventListener("resize",this.onWindowResize),n.cancel(this.raf))}calibrate(t,i){this.calibrateX=void 0===t?this.calibrateX:t,this.calibrateY=void 0===i?this.calibrateY:i}invert(t,i){this.invertX=void 0===t?this.invertX:t,this.invertY=void 0===i?this.invertY:i}friction(t,i){this.frictionX=void 0===t?this.frictionX:t,this.frictionY=void 0===i?this.frictionY:i}scalar(t,i){this.scalarX=void 0===t?this.scalarX:t,this.scalarY=void 0===i?this.scalarY:i}limit(t,i){this.limitX=void 0===t?this.limitX:t,this.limitY=void 0===i?this.limitY:i}origin(t,i){this.originX=void 0===t?this.originX:t,this.originY=void 0===i?this.originY:i}setInputElement(t){this.inputElement=t,this.updateDimensions()}setPosition(t,i,e){i=i.toFixed(this.precision)+"px",e=e.toFixed(this.precision)+"px",this.transform3DSupport?o.css(t,"transform","translate3d("+i+","+e+",0)"):this.transform2DSupport?o.css(t,"transform","translate("+i+","+e+")"):(t.style.left=i,t.style.top=e)}onOrientationTimer(){this.orientationSupport&&0===this.orientationStatus?(this.disable(),this.orientationSupport=!1,this.enable()):this.doReadyCallback()}onMotionTimer(){this.motionSupport&&0===this.motionStatus?(this.disable(),this.motionSupport=!1,this.enable()):this.doReadyCallback()}onCalibrationTimer(){this.calibrationFlag=!0}onWindowResize(){this.updateDimensions()}onAnimationFrame(){this.updateBounds();let t=this.inputX-this.calibrationX,i=this.inputY-this.calibrationY;(Math.abs(t)>this.calibrationThreshold||Math.abs(i)>this.calibrationThreshold)&&this.queueCalibration(0),this.portrait?(this.motionX=this.calibrateX?i:this.inputY,this.motionY=this.calibrateY?t:this.inputX):(this.motionX=this.calibrateX?t:this.inputX,this.motionY=this.calibrateY?i:this.inputY),this.motionX*=this.elementWidth*(this.scalarX/100),this.motionY*=this.elementHeight*(this.scalarY/100),isNaN(parseFloat(this.limitX))||(this.motionX=o.clamp(this.motionX,-this.limitX,this.limitX)),isNaN(parseFloat(this.limitY))||(this.motionY=o.clamp(this.motionY,-this.limitY,this.limitY)),this.velocityX+=(this.motionX-this.velocityX)*this.frictionX,this.velocityY+=(this.motionY-this.velocityY)*this.frictionY;for(let e=0;e<this.layers.length;e++)this.setPosition(this.layers[e],this.velocityX*(this.depthsX[e]*(this.invertX?-1:1)),this.velocityY*(this.depthsY[e]*(this.invertY?-1:1)));this.raf=n(this.onAnimationFrame)}rotate(t,i){let e=(t||0)/30,n=(i||0)/30,s=this.windowHeight>this.windowWidth;this.portrait!==s&&(this.portrait=s,this.calibrationFlag=!0),this.calibrationFlag&&(this.calibrationFlag=!1,this.calibrationX=e,this.calibrationY=n),this.inputX=e,this.inputY=n}onDeviceOrientation(t){let i=t.beta,e=t.gamma;null!==i&&null!==e&&(this.orientationStatus=1,this.rotate(i,e))}onDeviceMotion(t){let i=t.rotationRate.beta,e=t.rotationRate.gamma;null!==i&&null!==e&&(this.motionStatus=1,this.rotate(i,e))}onMouseMove(t){let i=t.clientX,e=t.clientY;if(this.hoverOnly&&(i<this.elementPositionX||i>this.elementPositionX+this.elementWidth||e<this.elementPositionY||e>this.elementPositionY+this.elementHeight))return this.inputX=0,void(this.inputY=0);this.relativeInput?(this.clipRelativeInput&&(i=Math.max(i,this.elementPositionX),i=Math.min(i,this.elementPositionX+this.elementWidth),e=Math.max(e,this.elementPositionY),e=Math.min(e,this.elementPositionY+this.elementHeight)),this.elementRangeX&&this.elementRangeY&&(this.inputX=(i-this.elementPositionX-this.elementCenterX)/this.elementRangeX,this.inputY=(e-this.elementPositionY-this.elementCenterY)/this.elementRangeY)):this.windowRadiusX&&this.windowRadiusY&&(this.inputX=(i-this.windowCenterX)/this.windowRadiusX,this.inputY=(e-this.windowCenterY)/this.windowRadiusY)}destroy(){this.disable(),clearTimeout(this.calibrationTimer),clearTimeout(this.detectionTimer),this.element.removeAttribute("style");for(let t=0;t<this.layers.length;t++)this.layers[t].removeAttribute("style");delete this.element,delete this.layers}version(){return"3.1.0"}}},4360:function(t){(function(){var i,e,n,s,o,r;"undefined"!=typeof performance&&null!==performance&&performance.now?t.exports=function(){return performance.now()}:"undefined"!=typeof process&&null!==process&&process.hrtime?(t.exports=function(){return(i()-o)/1e6},e=process.hrtime,s=(i=function(){var t;return 1e9*(t=e())[0]+t[1]})(),r=1e9*process.uptime(),o=s-r):Date.now?(t.exports=function(){return Date.now()-n},n=Date.now()):(t.exports=function(){return(new Date).getTime()-n},n=(new Date).getTime())}).call(this)},6712:(t,i,e)=>{for(var n=e(4360),s="undefined"==typeof window?global:window,o=["moz","webkit"],r="AnimationFrame",a=s["request"+r],h=s["cancel"+r]||s["cancelRequest"+r],l=0;!a&&l<o.length;l++)a=s[o[l]+"Request"+r],h=s[o[l]+"Cancel"+r]||s[o[l]+"CancelRequest"+r];if(!a||!h){var d=0,m=0,c=[];a=function(t){if(0===c.length){var i=n(),e=Math.max(0,16.666666666666668-(i-d));d=e+i,setTimeout(function(){var t=c.slice(0);c.length=0;for(var i=0;i<t.length;i++)if(!t[i].cancelled)try{t[i].callback(d)}catch(e){setTimeout(function(){throw e},0)}},Math.round(e))}return c.push({handle:++m,callback:t,cancelled:!1}),m},h=function(t){for(var i=0;i<c.length;i++)c[i].handle===t&&(c[i].cancelled=!0)}}t.exports=function(t){return a.call(s,t)},t.exports.cancel=function(){h.apply(s,arguments)},t.exports.polyfill=function(t){t||(t=s),t.requestAnimationFrame=a,t.cancelAnimationFrame=h}},4729:()=>{},9548:()=>{},1034:()=>{},6913:()=>{},838:()=>{}},t=>{"use strict";var i=i=>t(t.s=i);i(1034),i(6913),i(9548),i(4729),i(2492),i(838)}]);