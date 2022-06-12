import { Title } from "@mantine/core";
// @ts-ignore
import { Unity, useUnityContext } from "react-unity-webgl";
const MapColorizer = () => {
  const { unityProvider } = useUnityContext({
    loaderUrl: "build.loader.js",
    dataUrl: "build.data.gz",
    frameworkUrl: "build.framework.js.gz",
    codeUrl: "build.wasm.gz",
  });

  return (
    <section>
      <Title order={2}>Map</Title>
      <Unity
        unityProvider={unityProvider}
        style={{ height: "600px", width: "960px" }}
      />
    </section>
  );
};
export default MapColorizer;
