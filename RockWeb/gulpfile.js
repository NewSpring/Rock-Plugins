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
  		'./Themes/NewSpring1/**/*.less',
  		'!./Themes/NewSpring1/**/_*.less'
  	])
    .pipe(less())
    .pipe(gulp.dest('./Themes/NewSpring1'))
    .pipe(browserSync.stream());
});

gulp.task('sass', function(){
	gulp.src([
		'./Themes/NewSpring1/**/*.scss',
  		'!./Themes/NewSpring1/**/_*.scss'
	])
	.pipe(sass())
	.pipe(gulp.dest('./Themes/NewSpring1'))
	.pipe(browserSync.stream());
});

gulp.task('watch', function(){
	gulp.watch('Themes/NewSpring1/**/*.scss', ['sass']);
	gulp.watch('Themes/NewSpring1/**/*.less', ['less']);
});

gulp.task('build', ['less','sass']);

gulp.task('default', ['less','sass','watch','browser-sync']);