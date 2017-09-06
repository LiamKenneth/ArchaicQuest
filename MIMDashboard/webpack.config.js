/*
    ./webpack.config.js
*/
//process.env.NODE_ENV = 'production';
const path = require('path');
const webpack = require('webpack');
const OptimizeCssAssetsPlugin = require('optimize-css-assets-webpack-plugin');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const HtmlWebpackPluginConfig = new HtmlWebpackPlugin({
    template: './client/index.html',
    filename: 'index.html',
    inject: 'body'
});
const { CheckerPlugin } = require('awesome-typescript-loader')
const ExtractTextPlugin = require('extract-text-webpack-plugin');

module.exports = {
    devtool: 'eval-source-map',
    entry: ['./Client/index.js', './Client/Sass/style.scss'],
    resolve: {
        extensions: ['.ts', '.tsx', '.js', '.jsx']
    },
    output: {
        path: path.resolve('dist'),
        filename: 'index_bundle.js'
    },
    module: {

        loaders: [
            { test: /\.js$/, loader: 'babel-loader', exclude: /node_modules/ },
            { test: /\.jsx$/, loader: 'babel-loader', exclude: /node_modules/ },
            { test: /\.scss$/, loader: ExtractTextPlugin.extract(['css-loader', 'sass-loader']) },
            {
                test: /\.tsx?$/,
                loader: 'awesome-typescript-loader'
            }
        ]
    },
    //    externals: {
    //     "react": "React",
    //     "react-dom": "ReactDOM"
    // },

    plugins: [
        HtmlWebpackPluginConfig,
        new ExtractTextPlugin('styles.css'),
        new OptimizeCssAssetsPlugin({
            cssProcessorOptions: {
                safe: true,
            },
        }),
        new webpack.optimize.UglifyJsPlugin({
            compress: { warnings: false }
        }),
        new webpack.optimize.OccurrenceOrderPlugin(),
        new webpack.HotModuleReplacementPlugin(),
        new webpack.NoEmitOnErrorsPlugin(),
        new webpack.DefinePlugin({
            'process.env.NODE_ENV': JSON.stringify('development')
        }),
        new CheckerPlugin()
    ]
}
