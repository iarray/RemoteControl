package hph.app.UITest;

import hph.app.UITest.FileManagerActivity.ViewHolder;

import java.util.ArrayList;
import java.util.List;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.widget.AdapterView;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.AdapterView.OnItemClickListener;

public class PluginManagerActivity extends Activity{
	private ListView listView;
	private List<PluginBase> plugins;
	@Override
	public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        this.requestWindowFeature(Window.FEATURE_NO_TITLE);//去掉标题栏
        setContentView(R.layout.plugin);
        plugins=findPlugins("hph.app.PocketDesktop");
        listView=(ListView)findViewById(R.id.pluginlist);
        listView.setOnItemClickListener(new OnItemClickListener(){  
            public void onItemClick(AdapterView<?> parent, View view,int position, long id) {  
            	Intent it=new Intent();
				it.setAction(plugins.get(position).getPackageName()); 
				it.putExtra("server", AppConfig.getAppConfig().getServerHost());
				it.putExtra("port", AppConfig.getAppConfig().getServerPort());
				startActivity(it);
            }  
        }); 
        listView.setAdapter(new ListViewAdapter(plugins));
	}
	
	private List<PluginBase> findPlugins(String shareId){
		
		List<PluginBase> plugins=new ArrayList<PluginBase>();
		
		
		//遍历包名，来获取插件
		PackageManager pm=getPackageManager();
		
		
		List<PackageInfo> pkgs=pm.getInstalledPackages(PackageManager.GET_UNINSTALLED_PACKAGES);
		for(PackageInfo pkg	:pkgs){
			//包名
			String packageName=pkg.packageName;
			String sharedUserId= pkg.sharedUserId;
			
			//sharedUserId是开发时约定好的，这样判断是否为自己人
			if(!shareId.equals(sharedUserId)||"hph.app.UITest".equals(packageName))
				continue;
			
			
			//进程名
			//String prcessName=pkg.applicationInfo.processName;
			
			//label，也就是appName了
			String label=pm.getApplicationLabel(pkg.applicationInfo).toString();
			
			
			PluginBase plug=new PluginBase();
			plug.setPluginName(label);
			plug.setPackageName(packageName);
			plug.setIcon(pkg.applicationInfo.loadIcon(pm));
			plugins.add(plug);
		}
		return plugins;
	}
	
	 public class ListViewAdapter extends BaseAdapter { 
			List<PluginBase>  pgs;
			public ListViewAdapter (List<PluginBase> p) {
				pgs=p;
			}
			
			
			@Override
			public View getView(int position, View convertView, ViewGroup parent) {
				// TODO Auto-generated method stub
				ViewHolder holder;
				 
				 if (convertView == null) {
					 LayoutInflater inflater = (LayoutInflater) PluginManagerActivity.this    
			                    .getSystemService(Context.LAYOUT_INFLATER_SERVICE); 
					 convertView = inflater.inflate(R.layout.fileitem,parent, false);
					 holder = new ViewHolder();
					 holder.text = (TextView) convertView.findViewById(R.id.filePath);
					 holder.icon = (ImageView) convertView.findViewById(R.id.fileImg);
					 convertView.setTag(holder);
				} else {
					holder = (ViewHolder) convertView.getTag();
				}
				holder.text.setText(pgs.get(position).getPluginName());
				holder.icon.setImageDrawable(pgs.get(position).getIcon());
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
				return pgs.get(position);
			}
			
			@Override
			public int getCount() {
				// TODO Auto-generated method stub
				return pgs.size();
			}
		}
}
