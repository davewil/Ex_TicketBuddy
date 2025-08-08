import {defineProject, mergeConfig} from "vitest/config";
import viteConfig from './vite.config';
import {loadEnv} from "vite";
import react from "@vitejs/plugin-react";
import {fileURLToPath, pathToFileURL} from "node:url";

export default mergeConfig(viteConfig, defineProject({
    plugins: [react()],
    test: {
        environment: 'jsdom',
        env: loadEnv('testing', process.cwd(), ''),
    }
}));