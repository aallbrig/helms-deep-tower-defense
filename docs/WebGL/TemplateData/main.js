var canvas = document.querySelector("#unity-canvas");

var config = {
    dataUrl: "Build/WebGL.data",
    frameworkUrl: "Build/WebGL.framework.js",
    codeUrl: "Build/WebGL.wasm",
    streamingAssetsUrl: "StreamingAssets",
    companyName: "Andrew Allbright",
    productName: "helms-deep-tower-defense",
    productVersion: "0.0.13",
    devicePixelRatio: 1,
}

createUnityInstance(canvas, config);
