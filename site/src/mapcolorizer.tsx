import { Text, Title } from "@mantine/core";
// @ts-ignore
import { Unity, useUnityContext } from "react-unity-webgl";
const MapColorizer = () => {
  const { unityProvider, isLoaded } = useUnityContext({
    loaderUrl: "build.loader.js",
    dataUrl: "build.data",
    frameworkUrl: "build.framework.js",
    codeUrl: "build.wasm",
  });

  return (
    <section>
      <Title order={2}>Map</Title>
      {!isLoaded && <Text>Loading...</Text>}
      <Unity
        unityProvider={unityProvider}
        style={{ height: "600px", width: "960px" }}
      />
    </section>
  );
};
export default MapColorizer;
