var gulp = require('gulp');
var babel = require('gulp-babel');
var babelify = require('babelify');
var watch = require('gulp-watch');
var plumber = require('gulp-plumber');
var watchify = require('watchify');
var browserify = require('browserify');
var source = require('vinyl-source-stream');
var buffer = require('vinyl-buffer');
var gutil = require('gulp-util');
var sourcemaps = require('gulp-sourcemaps');
var uglify = require('gulp-uglify');
var sass = require('gulp-sass');
var collapse = require('bundle-collapser/plugin');
var del = require('del');
var postcss = require('gulp-postcss');
var autoprefixer = require('autoprefixer');

var babelSource = 'src/**/*.{js,jsx,react}';

gulp.task('clean:lib', function () {
  return del(['lib/**/*']);
});

gulp.task('compile:src', ['clean:lib'], function () {
  return gulp.src(babelSource)
    .pipe(babel())
    .pipe(gulp.dest('lib'));
});

gulp.task('watch:src', function () {
  return gulp.src(babelSource)
    .pipe(watch(babelSource))
    .pipe(plumber())
    .pipe(babel())
    .pipe(plumber.stop())
    .pipe(gulp.dest('lib'));
});

function watchJS (oldEntries, task, dest, fileName) {
  var entries = [];
  for (var i = 0; i < oldEntries.length; i++) {
    entries.push('./src' + oldEntries[i]);
  }

    // add custom browserify options here
  var customOpts = {
    entries: entries,
    debug: true
  };
  var opts = Object.assign({}, watchify.args, customOpts);
  var b = watchify(browserify(opts).transform(babelify));
  b.on('update', bundle); // on any dep update, runs the bundler
  // add transformations here
  b.on('log', gutil.log); // output build logs to terminal
  function bundle() {
    gutil.log('update detected in "' + task + '", compiling...');
    return b.bundle()
      // log errors if they happen
      .on('error', function (err) {
        gutil.log(gutil.colors.red('Browserify Error'), err.message);
      })
      .pipe(source(fileName + '.js'))
      // optional, remove if you don't need to buffer file contents
      .pipe(buffer())
      // optional, remove if you dont want sourcemaps
      .pipe(sourcemaps.init({loadMaps: true})) // loads map from browserify file
      // Add transformation tasks to the pipeline here.

      .pipe(sourcemaps.write('./')) // writes .map file
      .pipe(gulp.dest(dest));
  }
  gulp.task('watch:' + task, [], bundle);
}

function compileJS (oldEntries, task, dest, fileName) {
  var entries = [];
  for (var i = 0; i < oldEntries.length; i++) {
    entries.push('./lib' + oldEntries[i]);
  }
  // add custom browserify options here
  var customOpts = {
    entries: entries,
    debug: true
  };
  var opts = Object.assign({}, watchify.args, customOpts);
  var b = browserify(opts);
  b.plugin(collapse);
  // add transformations here

  b.on('log', gutil.log); // output build logs to terminal
  function bundle() {
    return b.bundle()
      // log errors if they happen
      .on('error', function (err) {
        gutil.log(gutil.colors.red('Browserify Error'), err.message);
      })
      .pipe(source(fileName + '.min.js'))
      // optional, remove if you don't need to buffer file contents
      .pipe(buffer())
      // optional, remove if you dont want sourcemaps
      .pipe(sourcemaps.init({loadMaps: true})) // loads map from browserify file
         // Add transformation tasks to the pipeline here.
      .pipe(uglify())

      .pipe(sourcemaps.write('./')) // writes .map file
      .pipe(gulp.dest(dest));
  }
  gulp.task('compile:' + task, ['compile:src'], bundle);
}

function watcher (entries, task, dest, fileName) {
  watchJS(entries, task, dest, fileName);
  compileJS(entries, task, dest, fileName);
}

var publicFolder = './Doctor + Design/www';

gulp.task('compile:sass', function () {
  gulp.src('./scss/**/*.scss')
    .pipe(sourcemaps.init())
    .pipe(plumber())
    .pipe(sass({outputStyle: 'compressed'}))
    .pipe(postcss([ autoprefixer({ browsers: ['last 2 versions'] }) ]))
    .pipe(plumber.stop())
    .pipe(sourcemaps.write())
    .pipe(gulp.dest(publicFolder + '/css'));
});
gulp.task('watch:sass', ['compile:sass'], function () {
  gulp.watch('./scss/**/*.scss', ['compile:sass']);
});

watcher(['/client/index.js'], 'web', publicFolder + '/js', 'index');

gulp.task('compile', ['compile:web', 'compile:sass']);
gulp.task('default', ['watch:web', 'watch:sass', 'watch:src']);
