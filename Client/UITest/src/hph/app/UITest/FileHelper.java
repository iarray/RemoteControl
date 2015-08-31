package hph.app.UITest;

import android.R.string;

public class FileHelper {
	 public static String getFileName(String filePathString)
	 {
		 int index=-1;
		 index = filePathString.lastIndexOf("\\");
		 if(index>0){
			 return filePathString.substring(index+1);
		 }
		 return null;
	 }
}
