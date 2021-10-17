const esbuild = require("esbuild");
const servor = require("servor");

const watch = "--watch";
const build = "--build";

var fsEntryPoint = './.fable-build/App.js'
var outdir = './public/scripts'
var webRoot = './public'
var port = 8080

//defaults to build
let config = build;
if (process.argv.length > 2) {
  config = process.argv[2];
}

config == watch &&
  esbuild.build({
    entryPoints: [fsEntryPoint],
    outdir: outdir,
    bundle: true,
    sourcemap: true,
    minify: false,
    watch: true,
    allowOverwrite: true,
    logLevel: 'error',
    define: { "process.env.NODE_ENV": '"development"' },
  });

config == build &&
  esbuild.build({
    entryPoints: [fsEntryPoint],
    outdir: outdir,
    bundle: true,
    minify: true,
    allowOverwrite: true,
    define: { "process.env.NODE_ENV": '"production"' },
  }) &&
  console.log("building for production");

// Run a local web server with livereload when -watch is set
config == watch && serve();

async function serve() {
  console.log("running server from: http://localhost:" + port + "/");
  await servor({
    browser: true,
    root: webRoot,
    port: port,
    fallback: 'index.html',
    reload: true,
  });
}