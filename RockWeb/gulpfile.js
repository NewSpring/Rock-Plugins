var gulp = require('gulp'),
	sass = require('gulp-sass'),
	less = require('gulp-less');

gulp.task('less', function () {
  return gulp.src('./Themes/KidSpring/**/*.less')
    .pipe(less())
    .pipe(gulp.dest('./Themes/KidSpring'))
});

gulp.task('sass', function(){
	gulp.src('./Plugins/**/*.scss')
	.pipe(sass())
	.pipe(gulp.dest('./Plugins'))
});

gulp.task('watch', function(){
	gulp.watch(['Themes/**/*.scss','Plugins/cc_newspring/**/*.scss'], ['sass']);
	gulp.watch('Themes/**/*.less', ['less']);
});

gulp.task('default', ['less','sass','watch']);