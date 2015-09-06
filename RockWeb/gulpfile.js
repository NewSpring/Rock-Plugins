var gulp = require('gulp'),
	sass = require('gulp-sass'),
	less = require('gulp-less'),
	browserSync = require('browser-sync').create();

gulp.task('browser-sync', function() {
    browserSync.init({
        proxy: "rock.dev:50345",
        xip: true,
        notify: true
    });
});

gulp.task('less', function () {
  	return gulp.src([
  		'./Themes/KidSpring/**/*.less',
  		'!./Themes/KidSpring/**/_*.less'
  	])
    .pipe(less())
    .pipe(gulp.dest('./Themes/KidSpring'))
    .pipe(browserSync.stream());
});

gulp.task('sass', function(){
	gulp.src([
		'./Themes/KidSpring/**/*.scss',
  		'!./Themes/KidSpring/**/_*.scss'
	])
	.pipe(sass())
	.pipe(gulp.dest('./Themes/KidSpring'))
	.pipe(browserSync.stream());
});

gulp.task('watch', function(){
	gulp.watch('Themes/KidSpring/**/*.scss', ['sass']);
	gulp.watch('Themes/KidSpring/**/*.less', ['less']);
});

gulp.task('build', ['less','sass']);

gulp.task('default', ['less','sass','watch','browser-sync']);