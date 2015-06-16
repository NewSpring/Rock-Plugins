var gulp = require('gulp'),
	sass = require('gulp-sass'),
	less = require('gulp-less'),
	browserSync = require('browser-sync').create();

gulp.task('browser-sync', function() {
    browserSync.init({
        proxy: "rock.dev:50345"
    });
});

gulp.task('less', function () {
  	return gulp.src([
  		'./Themes/NewSpring/**/*.less',
  		'!./Themes/NewSpring/**/_*.less'
  	])
    .pipe(less())
    .pipe(gulp.dest('./Themes/NewSpring'))
});

gulp.task('sass', function(){
	gulp.src([
		'./Themes/NewSpring/**/*.scss',
  		'!./Themes/NewSpring/**/_*.scss'
	])
	.pipe(sass())
	.pipe(gulp.dest('./Themes/NewSpring'))
});

gulp.task('watch', function(){
	gulp.watch('Themes/NewSpring/**/*.scss', ['sass']);
	gulp.watch('Themes/NewSpring/**/*.less', ['less']);
});

gulp.task('build', ['less','sass']);

gulp.task('default', ['less','sass','watch','browser-sync']);