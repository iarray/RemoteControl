package hph.app.UITest;

import java.util.ArrayList;
import java.util.List;
import java.util.Stack;

import android.R.integer;
import android.R.string;
import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Context;
import android.os.Bundle;
import android.view.ContextMenu;
import android.view.KeyEvent;
import android.view.LayoutInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.Window;
import android.view.ContextMenu.ContextMenuInfo;
import android.view.View.OnCreateContextMenuListener;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.PopupWindow;
import android.widget.TextView;
import android.widget.Toast;

public class FileManagerActivity extends Activity {
	static class ViewHolder {
		TextView text;
		ImageView icon;
		}
	
	private List<FileItem>Files;
	private ListView listView;
	private Stack<String> pathHistory;
	private String nowPath;
	 @Override
	    public void onCreate(Bundle savedInstanceState) {
	        super.onCreate(savedInstanceState);
	        this.requestWindowFeature(Window.FEATURE_NO_TITLE);//去掉标题栏
	        setContentView(R.layout.filelist);
	        listView=(ListView)findViewById(R.id.listView);
	        pathHistory=new Stack<String>();
	        nowPath="disk";
	        getList(nowPath,listView);
	        
	        listView.setOnItemClickListener(new OnItemClickListener(){  
	            public void onItemClick(AdapterView<?> parent, View view,int position, long id) {  
	            	entryDir(position); 
	            }  
	        });  
	        
	      //长按菜单显示  
	        listView.setOnCreateContextMenuListener(new OnCreateContextMenuListener() {
				
				@Override
				public void onCreateContextMenu(ContextMenu menu, View v,
						ContextMenuInfo menuInfo) {
					// TODO Auto-generated method stub
					menu.setHeaderTitle("菜单");  
					menu.add(0, 0, 0, "打开");  
					menu.add(0, 1, 1, "保存");  
				}
			})	;        
	    
	 }
	 
	 public void entryDir(int index) {
		 FileItem fileItem = Files.get(index);   //通过position获取所点击的对象  
         String filePath = fileItem.getPath();    //获取信息标题  
         if (fileItem.getFileType()==FileType.Dir||fileItem.getFileType()==FileType.Disk) {
				getList(filePath, listView);
				pathHistory.push(nowPath);
				nowPath=filePath;
			}
	}
	 
	 @Override
	public boolean onContextItemSelected(MenuItem item) {
		// TODO Auto-generated method stub
		 AdapterView.AdapterContextMenuInfo info = (AdapterView.AdapterContextMenuInfo)item.getMenuInfo();  
	        switch (item.getItemId()) {  
	             case 0:
	            	 entryDir(info.position); 
	                 return true; 
	             case 1:  
	            	 String savePath=AppConfig.getAppConfig().getSaveDirPath();
	            	//this表示该对话框是针对当前Activity的
	            	 ProgressDialog progressDialog = new ProgressDialog(this);
	            	 //设置进度条风格STYLE_HORIZONTAL
	            	 progressDialog.setProgressStyle(ProgressDialog.STYLE_HORIZONTAL);
	            	 progressDialog.setTitle("下载进度");
	            	 progressDialog.setCancelable(false);
	            	 progressDialog.incrementProgressBy(-progressDialog.getProgress());
	            	 //progressDialog.show();
	            	 saveFile(info.position,savePath,progressDialog);
	            	 // 创建一个PopuWidow对象
	            	 
	                 return true;  
	        }  
		return super.onContextItemSelected(item);
	}
	 
	 @Override
	    public boolean onKeyDown(int keyCode, KeyEvent event) {
	        if (keyCode == KeyEvent.KEYCODE_BACK) {
	               // Do something.
	        	if (pathHistory.size()>0) {
	        		String path=pathHistory.pop();
	        		getList(path, listView);
	        		nowPath=path;
	        		return true;
				}   
	        }
	        return super.onKeyDown(keyCode, event);
	    } 
	 
	 public void saveFile(int fileIndex,String savePathString,ProgressDialog pd) {
		FileItem fileItem=Files.get(fileIndex);
		if (fileItem.getFileType()==FileType.File) {
			FileRecieveEvent frecEvent=new FileRecieveEvent(getApplicationContext());
			frecEvent.setDownloadProgressDialog(pd);
			frecEvent.setMaxSize(fileItem.getSize());
			frecEvent.setSaveFilePath(savePathString+"/"+FileHelper.getFileName(fileItem.getPath()));
			SimpleSocketHelper.sendString(AppConfig.getAppConfig().getServerHost(), AppConfig.getAppConfig().getServerPort(), 
					"getfile|"+fileItem.getPath(), frecEvent);
		}
	}
	 
	 public void getList(String dirString,ListView listView){
		    StringRecieveEvent filerecvEvent=new StringRecieveEvent();
	        filerecvEvent.setCharSet("UTF-8");
	        SimpleSocketHelper.sendString(AppConfig.getAppConfig().getServerHost(), AppConfig.getAppConfig().getServerPort(), "getfilelist|"+dirString,filerecvEvent);
	        String fsString=(String)filerecvEvent.parameters;
	        if (fsString.length()<=0) {
				return;
			}
	        String[] pathsStrings=fsString.split(";");
	        Files=new ArrayList<FileItem>();
	        for (String path : pathsStrings) {
	        	String[] fileStrings=path.split(",");
	        	FileItem item=new FileItem();
	        	item.setPath(fileStrings[0]);
	        	item.setFileType(FileType.valueOf(fileStrings[1]));
	        	if (item.getFileType()==FileType.Disk) {
	        		item.setImgsrc(R.drawable.disk);	
				}else if (item.getFileType()==FileType.Dir) {
					item.setImgsrc(R.drawable.folder);	
				}else {
					item.setSize(Integer.valueOf(fileStrings[2]));
					item.setImgsrc(R.drawable.file);	
				}
	        	
				Files.add(item);
			}
	        listView.setAdapter(new ListViewAdapter(Files)); 
	 }
	 public class ListViewAdapter extends BaseAdapter { 
		List<FileItem>  fileItems;
		public ListViewAdapter (List<FileItem>files) {
			fileItems=files;
		}
		
		
		@Override
		public View getView(int position, View convertView, ViewGroup parent) {
			// TODO Auto-generated method stub
			ViewHolder holder;
			 
			 if (convertView == null) {
				 LayoutInflater inflater = (LayoutInflater) FileManagerActivity.this    
		                    .getSystemService(Context.LAYOUT_INFLATER_SERVICE); 
				 convertView = inflater.inflate(R.layout.fileitem,parent, false);
				 holder = new ViewHolder();
				 holder.text = (TextView) convertView.findViewById(R.id.filePath);
				 holder.icon = (ImageView) convertView.findViewById(R.id.fileImg);
				 convertView.setTag(holder);
			} else {
				holder = (ViewHolder) convertView.getTag();
			}
			holder.text.setText(fileItems.get(position).getPath());
			holder.icon.setImageResource(fileItems.get(position).getImgsrc());
			return convertView;
		}
		 @Override
		public long getItemId(int position) {
			// TODO Auto-generated method stub
			return position;
		}
		 @Override
		public Object getItem(int position) {
			// TODO Auto-generated method stub
			return fileItems.get(position);
		}
		
		@Override
		public int getCount() {
			// TODO Auto-generated method stub
			return fileItems.size();
		}
	}
}
