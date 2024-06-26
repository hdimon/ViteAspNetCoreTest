import { fileURLToPath, URL } from "node:url";

import { UserConfig, defineConfig } from "vite";
import { spawn } from "child_process";
import fs from "fs";
import path from "path";
import vue from "@vitejs/plugin-vue";
import vueJsx from "@vitejs/plugin-vue-jsx";
import { quasar, transformAssetUrls } from "@quasar/vite-plugin";
import cssInjectedByJsPlugin from "vite-plugin-css-injected-by-js";

// Get base folder for certificates.
const baseFolder =
  process.env.APPDATA !== undefined && process.env.APPDATA !== ""
    ? `${process.env.APPDATA}/ASP.NET/https`
    : `${process.env.HOME}/.aspnet/https`;

// Generate the certificate name using the NPM package name
const certificateName = process.env.npm_package_name;

// Define certificate filepath
const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
// Define key filepath
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

// Pattern for CSS files
const cssPattern = /\.css$/;
// Pattern for image files
const imagePattern = /\.(png|jpe?g|gif|svg|webp|avif)$/;

// https://vitejs.dev/config/

export default defineConfig(async () => {
  // Ensure the certificate and key exist
  if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
    // Wait for the certificate to be generated
    await new Promise<void>((resolve) => {
      spawn(
        "dotnet",
        [
          "dev-certs",
          "https",
          "--export-path",
          certFilePath,
          "--format",
          "Pem",
          "--no-password"
        ],
        { stdio: "inherit" }
      ).on("exit", (code) => {
        resolve();
        if (code) {
          process.exit(code);
        }
      });
    });
  }

  const root = "src";
  const entryPointName = "main.ts";
  // Specify the entry points and their output paths
  const entryPoints = {} as any;

  const mainFiles = findMainFiles(root, entryPointName);

  for (const file of mainFiles) {
    const relativePath = path.relative(root, file);
    const entryPoint = relativePath.replace(/\.ts$/, "");
    console.log(entryPoint + ":" + file);
    entryPoints[entryPoint] = file;
  }

  // Define Vite configuration
  const config: UserConfig = {
    appType: "custom",
    root: root,
    publicDir: "../public",
    build: {
      manifest: true,
      emptyOutDir: true,
      outDir: "../../wwwroot",
      assetsDir: "",
      rollupOptions: {
        input: entryPoints,
        output: {
          // Save entry files to the appropriate folder
          entryFileNames: "js/[name].[hash].js",
          /*entryFileNames: (info) => {
                        console.log(info);
                        return 'js/[name].[hash].js';
                    },*/
          // Save chunk files to the js folder
          //chunkFileNames: 'js/[name]-chunk.js',
          chunkFileNames: (info) => {
            //console.log(info);
            return "js/[name]-chunk.js";
          },
          // Save asset files to the appropriate folder
          assetFileNames: (info) => {
            if (info.name) {
              // If the file is a CSS file, save it to the css folder
              if (cssPattern.test(info.name)) {
                return "css/[name][extname]";
              }
              // If the file is an image file, save it to the images folder
              if (imagePattern.test(info.name)) {
                return "images/[name][extname]";
              }

              // If the file is any other type of file, save it to the assets folder
              return "assets/[name][extname]";
            } else {
              // If the file name is not specified, save it to the output directory
              return "[name][extname]";
            }
          }
        }
      }
    },
    server: {
      strictPort: true,
      https: {
        cert: certFilePath,
        key: keyFilePath
      }
    },
    optimizeDeps: {
      include: []
    },
    plugins: [
      vue({
        template: { transformAssetUrls }
      }),
      vueJsx(),
      quasar({
        sassVariables: "src/assets/quasar-variables.sass"
      }),
      cssInjectedByJsPlugin({ relativeCSSInjection: true })
    ],
    resolve: {
      alias: {
        "@": fileURLToPath(new URL("./src", import.meta.url))
      }
    }
  };

  return config;
});

function findMainFiles(dir: string, entryPointName: string): any[] {
  const mainFiles = [];
  const files = fs.readdirSync(dir);

  for (const file of files) {
    const filePath = path.join(dir, file);
    const stats = fs.statSync(filePath);

    if (stats.isDirectory()) {
      mainFiles.push(...findMainFiles(filePath, entryPointName)); // Recursively search subfolders
    } else if (stats.isFile() && file === entryPointName) {
      mainFiles.push(filePath);
    }
  }

  return mainFiles;
}
