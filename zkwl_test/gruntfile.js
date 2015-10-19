/// <binding AfterBuild='debug' />
// http://docs.asp.net/en/latest/client-side/using-grunt.html

module.exports = function (grunt) {
    grunt.initConfig({
        browserify: {
            debug: {
                src: 'app.jsx',
                dest: 'zkwl.bundle.js',
                options: {
                    transform: ['reactify'],
                    browserifyOptions: {
                        paths: ['./node_modules', '.', './node_modules/weblobby'],
                        debug: true,
                    },
                }
            },
            release: {
                src: 'app.jsx',
                dest: 'zkwl.bundle.js',
                options: {
                    transform: ['reactify'/*, 'uglifyify'*/],
                    browserifyOptions: {
                        paths: ['./node_modules', '.'],
                        debug: false,
                    },
                }
            },
        },
    });

    grunt.registerTask('debug', ['browserify:debug']);

    grunt.loadNpmTasks('grunt-browserify');
};