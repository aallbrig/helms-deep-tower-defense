var canvas = document.querySelector("#unity-canvas");

var config = {
    dataUrl: "Build/WebGL.data.unityweb",
    frameworkUrl: "Build/WebGL.framework.js.unityweb",
    codeUrl: "Build/WebGL.wasm.unityweb",
    streamingAssetsUrl: "StreamingAssets",
    companyName: "Andrew Allbright",
    productName: "helms-deep-tower-defense",
    productVersion: "0.0.12",
    devicePixelRatio: 1,
}

createUnityInstance(canvas, config);
