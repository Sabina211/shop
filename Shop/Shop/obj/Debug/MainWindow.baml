   M S B A M L   `   `   `  ����?  ;Shop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ShopShop  5   	       Shop.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   $localclr-namespace:Shop     5   	     � Title$  
MainWindow��5   	   $	��561��6   $	��926��6)   .��5	      � .+�5
   
   �� -   5         ��Name$ 	MsSqlName��5
      $
��Left=�6%   $��41��6@   $��
185,17,0,0q�6L    �� TextWrapping$
 Wrap=�6`   $	��Top=�5   ^   $	��723��6v   ��6   �� *MsSqlConnectingString+ �� UpdateSourceTrigger$ PropertyChanged=�6v   �� 5   
   -   5        $ 
AccessName��5      $
��Left=�6&   $��41��6A   $��
185,63,0,0q�6M   $
 Wrap=�6a   $	��Top=�5   _   $	��723��6w   ��6   �� *AccessConnectingString+$ PropertyChanged=�6w   �� 5   
   $*��$Connectong string/State 
for MS SQL��6   $
��Left=�6G   $��	10,17,0,0q�6b   $	��Top=�6u   $	��170��6�   $��41��6�   �� 5   
   $*��$Connectong string/State 
for Access��6   $
��Left=�6G   $��	10,63,0,0q�6b   $	��Top=�6u   $	��170��6�   $��41��6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ gridView��5      $ �� IsSynchronizedWithCurrentItem$
 TrueY�5         SelectionMode$ Single=�5         AutoGenerateColumns .     
IsReadOnly .  $��10,184,424,200q�5      	 J� RenderTransformOrigin$	 0.499,0.688&�6+   ��5      �� *ClientsData+
 �� SelectedItem
 5      �� *SelectedClient+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      -   5        id��,  &System.Windows.Controls.DataGridColumn   Header$ Id��5   %      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 5   %   �� *id+5   %     5      -   5        surname��$ Фамилия��5!   %    5    %   �� *	surname+5!   %     5#      -   5        
first_name��$ Имя��5%   %    5$   %   �� *
first_name+5%   %     5'      -   5        
patronymic��$ Отчество��5)   %    5(   %   �� *
patronymic+5)   %     5+      -   5        phone_number��$ Телефон��5-   %    5,   %   �� *phone_number+5-   %     5/      -	   5        	email��$ 
Почта��51   %    50   %   �� *email+51   %    � ContextMenu 53      �� 54      .@�55      m� $��Добавить��6    m� Command 61   �� *AddClientCommand+m� 56      $��Удалить��6    60   �� *DeleteClientCommand+m� 57      $��Изменить��6    61   �� *EditClientCommand+58      5:        5<   
   -
   5        $ gridProducts��5<      $
 TrueY�5=      $ Single=�5>       .   .  $��10,364,10,25q�5B      ��5@      �� *ProductsData+ 5D        5E      -   5        
product_id��$ Id��5G   %    .  5F   %   �� *id+5H   %     5J      -   5        product_email��$ 
Почта��5L   %    5K   %   �� *email+5L   %     5N      -   5        product_code��$ Код товара��5P   %    5O   %   �� *product_code+5P   %     5R      -   5        product_name��$ Наименование��5T   %    5S   %   �� *product_name+5T   %   5V        5W   
   -   5        $ selectedClientProduct��5W      $
 TrueY�5X      $ Single=�5Y       .   .  $��508,184,10,200q�5^      ��5[      �� *ClientsProductsData+
 5\      �� *SelectedProduct+ 5`        5a      -   5        product_id1��$ Id��5c       .  5b      �� *id+5d        5f      -   5        product_email1��$ 
Почта��5h       5g      �� *email+5h        5j      -   5        product_code1��$ Код товара��5l       5k      �� *product_code+5l        5n      -   5        product_name1��$ Наименование��5p       5o      �� *product_name+5p       5r      �� 5s      .@�5t      m� $��Добавить��6    61   �� *AddProductCommand+m� 5u      $��Удалить��6    61   �� *DeleteProductCommand+5v      5x      �� 5y   
   $#��Список клиентов��6   $
��Left=�6*   $��
10,121,0,0q�6E   $	��Top=�6Y   $	 -0.053,0.192&�6q   $	��170��6�   $
��Bold"�6�   $��14(�6�   �� 5z   
   $6��0Товары выбранного клиента��6   $
��Left=�64   $��508,121,0,0q�6O   $	��Top=�6d   $	 -0.053,0.192&�6|   $	��268��6�   $
��Bold"�6�   $��14(�6�   �� 5{   
   $!��Все товары в БД��6   $
��Left=�6*   $��
10,335,0,0q�6E   $	��Top=�6Y   $	 -0.053,0.192&�6q   $	��170��6�   $
��Bold"�6�   $��14(�6�   �� 5|   
   $����Для добавления, удаления и редактирования клиентов используйте 
контекстное меню��6   $
��Left=�6t   $��
10,143,0,0q�6�   $	��Top=�6�   $	 0.105,0&�6�   �� 5}   
   $����Для добавления и удаления товаров клентов используйте 
контекстное меню��6   $
��Left=�6f   $��508,143,0,0q�6�   $	��Top=�6�   $	 0.105,0&�6�   5~      5      