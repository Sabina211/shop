   M S B A M L   `   `   `  ����?  ;Shop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ShopShop  5   	       Shop.AddProductWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   $localclr-namespace:Shop     5   	     � Title$%  Добавить покупку��5   	   $��273.17��6"   $��407.317��62   .��5	      � $
��Left=�6   $	��422��6&   $��Bottom=�63   $	��451��6N   $��
0,0,-75,-3q�6Z    � ColumnDefinitions 5
   
   �� 5      $	��32* �6   �� 5      $	��57* �6    � RowDefinitions 5   
   �� 5      .+�5   
   �� $��Код товара��6   $
��Left=�6%   $��
16,191,0,0q�6@   $	��Top=�6T    J� RenderTransformOrigin$ -0.759,-0.815&�6l   $��26��6�   $��82��6�   $��14(�6�   �� 5   
   $+��%Наименование товара��6   $
��Left=�6.   $��
16,221,0,0q�6I   $	��Top=�6]   $ -1.129,-2.676&�6u   $��26��6�   $	��157��6�   $��14(�6�   $��2��6�   �� 5   
   $��Почта клиента��6   $
��Left=�6(   $��
15,250,0,0q�6C   $	��Top=�6W   $��27��6o   $	��137��6{   $��14(�6�   �� 5   
   $��1��6   $��11,186,71,84q�6%   .+�5      �� -   5         ��Name$ ProductCode��5      $
��Left=�5      $��23��61   $��	10,10,0,0q�6=    �� TextWrapping$
 Wrap=�6P   $	��Top=�6d   $	��193��6|   ��5      �� *AddedProduct.ProductCode+ �� UpdateSourceTrigger$ PropertyChanged=�5   |   �� 5      -   5        $ ProductName��5      $��1��5      $
��Left=�6&   $��23��6A   $��10,5,0,0q�6M   $
 Wrap=�6_   $	��Top=�6s   $	��193��6�   ��5      �� *AddedProduct.ProductName+$ PropertyChanged=�5   �   �� 5      -   5        $ Email��5      $��1��5      $
��Left=�6&   $��23��6A   $��10,5,0,0q�6M   $
 Wrap=�6_   $	��Top=�6s   $	��193��6�   ��5      �� *AddedProduct.Email+$ PropertyChanged=�5   �   5      �� 5   
   $��Сохранить��6   $��1��5!      $
��Left=�6%   $��116,359,0,0q�6@   $	��Top=�6U   $	��102��6m   $��30��6y   $��14(�6�   ��5       �� *SaveCommand+5!   �   �� 5"   
   $:��4Заполните обязательные поля��5#      $��1��63   $
��Left=�6C   $��
21,311,0,0q�6^   $	��Top=�6r   $��13(�6�   ���	#FFB90606y�5"      �� *ErrorEnable+$ PropertyChanged=���5#   �   Z� 6�   5$      5%      