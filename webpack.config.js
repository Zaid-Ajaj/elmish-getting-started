const path = require("path")
const DotenvPlugin = require("dotenv-webpack")
var webpack = require("webpack");

module.exports = (env, argv) => {
    // extract build mode from command-line
    const mode = argv.mode
    console.log(mode);

    return {
        mode: mode,
        entry: "./src/App.fsproj",
        devServer: {
            contentBase: path.join(__dirname, "./dist"),
            hot: true,
            inline: true
        },
        plugins: mode === "development" ?
            // development mode plugins
            [
                new DotenvPlugin({
                    path: path.join(__dirname, ".env"),
                    silent: true,
                    systemvars: true
                }),

                new webpack.HotModuleReplacementPlugin()
            ]
            :
            // production mode plugins
            [
                new DotenvPlugin({
                    path: path.join(__dirname, ".env"),
                    silent: true,
                    systemvars: true
                })
            ],
        module: {
            rules: [{
                test: /\.fs(x|proj)?$/,
                use: {
                    loader: "fable-loader",
                    options: {
                        define: mode === "development" ? ["DEVELOPMENT"] : []
                    }
                }
            }]
        }
    }
}