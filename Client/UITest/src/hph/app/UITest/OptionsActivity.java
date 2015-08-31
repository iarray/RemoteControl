package hph.app.UITest;

import hph.app.UITest.AppConfig.ConnectModel;
import android.R.bool;
import android.R.integer;
import android.app.Activity;
import android.content.Context;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;
import android.os.Bundle;
import android.os.Environment;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.Window;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.EditText;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.Toast;
import android.widget.RadioGroup.OnCheckedChangeListener;
import android.widget.SeekBar;


public class OptionsActivity extends Activity {
	
	private SeekBar mouseRangeSeekBar;
	private SeekBar imageQualitySeekBar;
	private EditText savePathEditText;
	private EditText serverPortEditText;
	private EditText ipEditText;
	private boolean isPublicNetwork=false;
	@Override
	public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        this.requestWindowFeature(Window.FEATURE_NO_TITLE);//去掉标题栏
        setContentView(R.layout.options);
        AppConfig.loadOptions(OptionsActivity.this);
        
        mouseRangeSeekBar=(SeekBar)findViewById(R.id.sbMouseMoveRange);
        mouseRangeSeekBar.setProgress(AppConfig.getAppConfig().getMoveRange());
        mouseRangeSeekBar.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
            /**
             * 拖动条停止拖动的时候调用
             */
            @Override
            public void onStopTrackingTouch(SeekBar seekBar) {
                
            }
            /**
             * 拖动条开始拖动的时候调用
             */
            @Override
            public void onStartTrackingTouch(SeekBar seekBar) {
                
            }
            /**
             * 拖动条进度改变的时候调用
             */
            @Override
            public void onProgressChanged(SeekBar seekBar, int progress,
                    boolean fromUser) {
            	AppConfig.getAppConfig().setMoveRange(progress);
            }
        });
        
        imageQualitySeekBar=(SeekBar)findViewById(R.id.sbImageQuality);
        imageQualitySeekBar.setProgress(AppConfig.getAppConfig().getIntQuality());
        imageQualitySeekBar.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
            /**
             * 拖动条停止拖动的时候调用
             */
            @Override
            public void onStopTrackingTouch(SeekBar seekBar) {
                
            }
            /**
             * 拖动条开始拖动的时候调用
             */
            @Override
            public void onStartTrackingTouch(SeekBar seekBar) {
                
            }
            /**
             * 拖动条进度改变的时候调用
             */
            @Override
            public void onProgressChanged(SeekBar seekBar, int progress,
                    boolean fromUser) {
            	AppConfig.getAppConfig().setQuality(progress);
            }
        });
        
        RadioGroup showModelGroup=(RadioGroup)findViewById(R.id.rgShowModel);
        RadioButton followMouseButton=(RadioButton)findViewById(R.id.rbFallowMouse);
        RadioButton fullScreenButton=(RadioButton)findViewById(R.id.rbFullScreen);
        showModelGroup.setOnCheckedChangeListener(new OnCheckedChangeListener(){
        	 @Override
             public void onCheckedChanged(RadioGroup arg0, int arg1) {
                 // TODO Auto-generated method stub
                 //获取变更后的选中项的ID
                 int radioButtonId = arg0.getCheckedRadioButtonId();
                 //根据ID获取RadioButton的实例
                 switch (radioButtonId) {
				case R.id.rbFallowMouse:
					AppConfig.getAppConfig().setShowModel("FollowMouse");
					break;

				case R.id.rbFullScreen:
					AppConfig.getAppConfig().setShowModel("FullScreen");
					break;
				default:
					AppConfig.getAppConfig().setShowModel("FollowMouse");
					break;
				}
                 
             }

        });
        if (AppConfig.getAppConfig().getShowModel()=="FullScreen") {
			fullScreenButton.setChecked(true);
		}else {
			followMouseButton.setChecked(true);
		}
        
        savePathEditText =(EditText)findViewById(R.id.etSavePath);
        savePathEditText.setText(AppConfig.getAppConfig().getSaveDirPath());
        
        serverPortEditText=(EditText)findViewById(R.id.etPort);
        serverPortEditText.setText(Integer.toString(AppConfig.getAppConfig().getServerPort()));
        
        ipEditText=(EditText)findViewById(R.id.etIP);
        
        RadioGroup connectModelGroup=(RadioGroup)findViewById(R.id.rgConnect);
        connectModelGroup.setOnCheckedChangeListener(new OnCheckedChangeListener(){
       	 @Override
            public void onCheckedChanged(RadioGroup arg0, int arg1) {
                // TODO Auto-generated method stub
                //获取变更后的选中项的ID
                int radioButtonId = arg0.getCheckedRadioButtonId();
                //根据ID获取RadioButton的实例
                switch (radioButtonId) {
				case R.id.rbPublicNetwork:
					ipEditText.setEnabled(true);
					isPublicNetwork=true;
					break;

				case R.id.rbLocalNetwork:
					ipEditText.setEnabled(false);
					isPublicNetwork=false;
					break;
				default:
					isPublicNetwork=false;
					ipEditText.setEnabled(false);
					break;
				}
                
            }

       });
        Button saveButton=(Button)findViewById(R.id.btnSave);
        saveButton.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				AppConfig.getAppConfig().setSaveDirPath(savePathEditText.getText().toString());
				int port=Integer.valueOf(serverPortEditText.getText().toString());
				if (port<65536&&port>1024) {
					AppConfig.getAppConfig().setServerPort(port);	
				}
				else {
					Toast.makeText(getApplicationContext(), "端口范围应该为1024~65536之间,且与服务端端口一致", Toast.LENGTH_SHORT).show();
					return;
				}
				if (isPublicNetwork) {
					AppConfig.getAppConfig().setConnectModel(ConnectModel.PublicNetwork);
					AppConfig.getAppConfig().setServerHost(ipEditText.getText().toString());
				}else {
					AppConfig.getAppConfig().setConnectModel(ConnectModel.LocalNetwork);
				}
				saveOptions();
				OptionsActivity.this.finish();
			}
		});
	}
	
	
	private void saveOptions() {

		//获取SharedPreferences对象
		       Context ctx = OptionsActivity.this;      
		       SharedPreferences sp = ctx.getSharedPreferences("Options", MODE_PRIVATE);
		//存入数据
		       Editor editor = sp.edit();
		       editor.putInt("MouseMoveRange",AppConfig.getAppConfig().getMoveRange());
		       editor.putInt("ImageQuality", AppConfig.getAppConfig().getIntQuality());
		       editor.putString("ShowModel", AppConfig.getAppConfig().getShowModel());
		       editor.putString("SavePath", AppConfig.getAppConfig().getSaveDirPath());
		       editor.putInt("ServerPort", AppConfig.getAppConfig().getServerPort());
		       editor.commit();
	}
	
}
